using System;
using System.Collections.Generic;
using Holoville.HOTween;
using TouchScript.Events;
using TouchScript.Gestures;
using UniKid.Core;
using UniKid.SubGame.Controller;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.TexturePaintGame.View
{
    public sealed class Brush : EventView
    {
        public enum BrushModeType
        {
            Autoplay,
            HalfHand,
            Hand
        }

        public enum BrushEventType
        {
            PaintCharIsCompleted,
            PaintCharPartIsCompleted
        }

        public tk2dSprite Head;
        public BrushModeType BrushMode;

        [SerializeField] private tk2dSprite _background;
        [SerializeField] private GameObject _wayTrail;
        [SerializeField] private float _wayTrailSpeed = 3;
        [SerializeField] private Transform _completedCharPosition;
        [SerializeField] private float _completedCharScaleFactor = 0.5f;

        private bool IsInsidePaintChar { get; set; }

        private TouchScript.Gestures.TapGesture _tapGesture;
        private PaintChar _paintChar;

        private TouchScript.Gestures.TapGesture TapGesture
        {
            get
            {
                if (_tapGesture != null) return _tapGesture;
                return _paintChar.CurrentPaintCharPart.GetComponent<TouchScript.Gestures.TapGesture>();
            }
            set { _tapGesture = value; }
        }

        private Transform _transform;
        private TexturePaintInfluenceActor _actor;
        private readonly List<Waypoint> _waypointList = new List<Waypoint>();

        private RaycastHit _hit;
        private Vector3 _lastHitLocation = Vector3.zero;
        private TexturePaintScript _texturePaint;
        private GameObject _halfHandPositionObject;
        private float _headMovedDistance;
        private bool _beganMoveIsOk;
        private bool _isCompleted;

        private bool _isInited = false;

        private Vector3? _startBrushPosition;

        private TexturePaintScript TexturePaint
        {
            get
            {
                if (_texturePaint != null) return _texturePaint;
                _texturePaint = (TexturePaintScript)FindObjectOfType(typeof(TexturePaintScript));
                return _texturePaint;
            }
        }


        private Waypoint CurrentWaypoint { get; set; }
        private Waypoint LastControlPoint { get; set; }

        public void Init(PaintChar paintChar)
        {
            _paintChar = paintChar;
            _transform = transform;
            _actor = GetComponent<TexturePaintInfluenceActor>();
            _actor.colorDetails.addColor = false;

            _paintChar.FirstWaypoint.IsControlPoint = true;
            CurrentWaypoint = LastControlPoint = _paintChar.FirstWaypoint;

            _waypointList.AddRange(_paintChar.Waypoints.GetComponentsInChildren<Waypoint>());

            if (BrushMode == BrushModeType.Hand)
            {
                _paintChar.AddTapGestureComponent(TapGestureOnStateChanged);
                _background.gameObject.AddComponent<TouchScript.Hit.Untouchable.Untouchable>();
                _background.gameObject.GetComponent<BoxCollider>().enabled = false;
            }

            if (BrushMode == BrushModeType.HalfHand)
            {
                TapGesture = _background.gameObject.AddComponent<TouchScript.Gestures.TapGesture>();
                TapGesture.StateChanged += TapGestureOnStateChanged;

                _paintChar.AddComponentToParts(typeof(TouchScript.Hit.Untouchable.Untouchable));
                _halfHandPositionObject = new GameObject("HalfHandPositionObject");
            }

            ResetHeadPosition();


            TexturePaint.syncWithUpdate = true;

            if (BrushMode == BrushModeType.Autoplay) RunAutoplay();

            _paintChar.MoveNextCharPart();

            RunWayTrail(null);

            _isInited = true;
            _isCompleted = false;
        }


        private void TapGestureOnStateChanged(object sender, GestureStateChangeEventArgs args)
        {
            if (args.State != TouchScript.Gestures.Gesture.GestureState.Possible) return;

            if (!DetectNextWaypoint()) MoveHeadBack();
        }

        void FixedUpdate()
        {
            if (!_isInited || BrushMode == BrushModeType.Autoplay || TapGesture == null || _isCompleted) return;

            _actor.colorDetails.addColor = false;

            if (TapGesture.State == TouchScript.Gestures.Gesture.GestureState.Changed || TapGesture.State == TouchScript.Gestures.Gesture.GestureState.Began)
            {
                var ray = Camera.main.ScreenPointToRay(TapGesture.ScreenPosition);

                if (Physics.Raycast(ray, out _hit)) _lastHitLocation = _hit.point;
                
                if (IsInsidePaintChar)
                {
                    if (BrushMode == BrushModeType.Hand)
                    {
                        var newBrushPos = new Vector3(_lastHitLocation.x, _lastHitLocation.y, _transform.localPosition.z);
                        var wpPos = new Vector3(CurrentWaypoint.transform.localPosition.x, CurrentWaypoint.transform.localPosition.y, newBrushPos.z);

                        if (TapGesture.State == TouchScript.Gestures.Gesture.GestureState.Began && Vector3.Distance(wpPos, newBrushPos) > 1.5f) // if began move is not so far from current waypoint
                        {
                            _beganMoveIsOk = false;
                            return; 
                        }

                        if (TapGesture.State == TouchScript.Gestures.Gesture.GestureState.Changed && !_beganMoveIsOk) return;

                        _beganMoveIsOk = true;

                        _actor.colorDetails.addColor = true; 

                        var brushPosDiff = _transform.localPosition - newBrushPos;
                        
                        _transform.localPosition -= brushPosDiff * Time.fixedDeltaTime * 6;

                        //TexturePaint.ForceColorUpdate(); 

                        Head.transform.localPosition = new Vector3(_lastHitLocation.x, _lastHitLocation.y, Head.transform.localPosition.z);
                    }
                    else
                    {
                        _actor.colorDetails.addColor = true; 

                        MoveHalfHandHead(_lastHitLocation);
                        Head.transform.localPosition = new Vector3(_lastHitLocation.x, _lastHitLocation.y, Head.transform.localPosition.z);
                    }
                }
                

                DetectNextWaypoint();

                if (TapGesture.State == TouchScript.Gestures.Gesture.GestureState.Changed && _startBrushPosition == null) _startBrushPosition = _transform.localPosition;
            }

        }

        public void BrushPaintCharColliderTrigger(bool isEnter)
        {
            if (BrushMode == BrushModeType.Autoplay) return;

            IsInsidePaintChar = isEnter || BrushMode == BrushModeType.HalfHand;

            if (!isEnter && BrushMode == BrushModeType.Hand) MoveHeadBack();
        }

        private void MoveHeadBack()
        {
            ResetHeadPosition();

            _beganMoveIsOk = false;

            TexturePaint.RevertToLastSnapshot();
        }

        private void ResetHeadPosition()
        {
            CurrentWaypoint = LastControlPoint;

            if (CurrentWaypoint == null) return;

            _startBrushPosition = null;

            var pos = CurrentWaypoint.transform.localPosition;

            Head.transform.localPosition = new Vector3(pos.x, pos.y, Head.transform.localPosition.z);
            _transform.localPosition = new Vector3(pos.x, pos.y, _transform.localPosition.z);

            if (_halfHandPositionObject != null) _halfHandPositionObject.transform.localPosition = new Vector3(pos.x, pos.y, _transform.localPosition.z);
        }

        private bool DetectNextWaypoint()
        {
            if (!IsBrushOverNextWaypoint()) return false;

            if (CurrentWaypoint.NextWaypoint == null) throw new Exception("Next waypoint of " + CurrentWaypoint.name + " is null ");

            CurrentWaypoint = CurrentWaypoint.NextWaypoint;
            
            if (CurrentWaypoint.IsControlPoint)
            {
                TexturePaint.MakeTextureSnapshot();
            }

            if (!CurrentWaypoint.IsConnectedWithNext) CurrentWaypoint = CurrentWaypoint.NextWaypoint;

            if (CurrentWaypoint.IsControlPoint)
            {
                LastControlPoint = CurrentWaypoint;

                if (LastControlPoint.IsStartOfNewPart) dispatcher.Dispatch(BrushEventType.PaintCharPartIsCompleted);
                ResetHeadPosition();
            }
            
            if (CurrentWaypoint.NextWaypoint == null)
            {
                dispatcher.Dispatch(BrushEventType.PaintCharIsCompleted);

                _actor.colorDetails.addColor = false;
                _actor.enabled = false;
                _isCompleted = true;
                _isInited = false;
            }

            return true;
        }

        private bool IsBrushOverNextWaypoint()
        {
            if (CurrentWaypoint.NextWaypoint == null || !_startBrushPosition.HasValue) return false;

            var brushMovedDistance = Vector3.Distance(_transform.localPosition, _startBrushPosition.Value);
            var waypointDistance = Vector3.Distance(CurrentWaypoint.transform.localPosition, CurrentWaypoint.NextWaypoint.transform.localPosition);

            var isOver = Mathf.Abs(brushMovedDistance - waypointDistance) < 0.25f;
            
            if (isOver) _startBrushPosition = null;

            return isOver;
        }
        

        private void RunWayTrail(TweenEvent te)
        {
            if (_wayTrail == null) return;
            
            var currentWaypoint = te != null && te.parms[0] != null ? (Waypoint)te.parms[0] : CurrentWaypoint;

            if (currentWaypoint.IsControlPoint && currentWaypoint.GetInstanceID() != CurrentWaypoint.GetInstanceID()) currentWaypoint = CurrentWaypoint;
            
            if (currentWaypoint.NextWaypoint == null || BrushMode == BrushModeType.Autoplay)
            {
                _wayTrail.SetActive(false);
                return;
            }

            var currentWpPos = currentWaypoint.transform.localPosition;

            var initPos = new Vector3(currentWpPos.x, currentWpPos.y, _wayTrail.transform.localPosition.z);
            _wayTrail.transform.localPosition = initPos;

            var nextWpPos = currentWaypoint.NextWaypoint.transform.localPosition;
            var nextPos = new Vector3(nextWpPos.x, nextWpPos.y, initPos.z);

            #region animator rotatation calculating

            var sinA = Vector3.Magnitude(Vector3.Project(nextPos - initPos, Vector3.left)) / Vector3.Magnitude(nextPos - initPos);

            if (nextWpPos.y <= currentWpPos.y) sinA = (nextWpPos.x < currentWpPos.x) ? -sinA : sinA;
            else if (nextWpPos.x > currentWpPos.x) sinA = -sinA;

            var angle = Mathf.Asin(sinA) * 180 / Mathf.PI;

            if (nextWpPos.y > currentWpPos.y) angle = angle + 180;

            _wayTrail.transform.localRotation = Quaternion.Euler(0, 0, angle);

            #endregion

            var distance = Vector3.Distance(new Vector2(currentWpPos.x, currentWpPos.y), new Vector2(nextWpPos.x, nextWpPos.y));

            var param = new TweenParms();

            param.Prop("localPosition", nextPos);
            param.Ease(EaseType.Linear);
            param.OnComplete(RunWayTrail, currentWaypoint.NextWaypoint);

            _wayTrail.gameObject.SetActive(TapGesture.State == TouchScript.Gestures.Gesture.GestureState.Possible);

            HOTween.To(_wayTrail.transform, distance / _wayTrailSpeed, param);
        }

        private void RunAutoplay()
        {
            if (CurrentWaypoint.NextWaypoint == null) return;

            _actor.colorDetails.addColor = CurrentWaypoint.IsConnectedWithNext;

            var nextWaypointPos = CurrentWaypoint.NextWaypoint.transform.localPosition;
            var endPoint = new Vector3(nextWaypointPos.x, nextWaypointPos.y, Head.transform.localPosition.z);

            var param = new TweenParms();

            param.Prop("localPosition", endPoint);
            param.Ease(EaseType.Linear);
            param.OnComplete(() => { CurrentWaypoint = CurrentWaypoint.NextWaypoint; RunAutoplay(); });
            param.OnUpdate(() => _transform.localPosition = Head.transform.localPosition);

            var distance = Vector3.Distance(Head.transform.localPosition, endPoint);

            HOTween.To(Head.transform, distance / _wayTrailSpeed, param);
        }
        
        private void MoveHalfHandHead(Vector3 inputPosition)
        {
            if (float.IsNaN(inputPosition.x) || float.IsNaN(inputPosition.y) || float.IsNaN(inputPosition.z)) return;
            if (CurrentWaypoint.NextWaypoint == null) return;

            var wp0 = CurrentWaypoint.transform.localPosition;
            var wp1 = CurrentWaypoint.NextWaypoint.transform.localPosition;

            var waypointDirection = wp1 - wp0;
            var zPos = _halfHandPositionObject.transform.localPosition.z;

            //wp1 - wp0 - direction of waypoints
            //Input.mousePosition - wp0 - mouse position about start point
            var project = Vector3.Project(inputPosition - wp0, waypointDirection);

            _halfHandPositionObject.transform.localPosition = wp0;

            _halfHandPositionObject.transform.Translate(project);

            var cursorDicrection = _halfHandPositionObject.transform.localPosition - wp0;

            if (Vector3.Angle(cursorDicrection, waypointDirection) < 0.01f) // if vectors is co-direction
                if (cursorDicrection.sqrMagnitude < waypointDirection.sqrMagnitude) // if cursor direction vector is less than waypoint direction vector
                {
                    var pos = _halfHandPositionObject.transform.localPosition;
                    _transform.localPosition = new Vector3(pos.x, pos.y, zPos);
                }
        }
        
        public void DestroyPaint(float duration)
        {
            if (_completedCharPosition == null) return;

            var initScale = _texturePaint.transform.localScale;

            var param = new TweenParms();
            
            param.Prop("localScale", Vector3.zero);
            param.Ease(EaseType.Linear);
            param.OnComplete(() => { TexturePaint.ClearColorBufferToTexture(); _texturePaint.transform.localScale = initScale; });

            HOTween.To(_texturePaint.transform, duration, param);
        }

    }
}

