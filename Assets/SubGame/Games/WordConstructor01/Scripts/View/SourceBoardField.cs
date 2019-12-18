using Holoville.HOTween;
using UnityEngine;

namespace UniKid.SubGame.Games.WordConstructor01.View
{
    public sealed class SourceBoardField : BoardFieldBase
    {
        public static readonly float MOVE_DURATION = 1f;

        public enum ViewEventType
        {
            DragIsBegan,
            DragIsFinished
        }

        private Vector3 _dragStartPosition;
        private int _dragFingerIndex = -1;
        private GameObject _dragObject;
        private float _dragObjectZ;
        private bool _movedOnTarget;
        
        private void OnDrag(DragGesture gesture)
        {
            FingerGestures.Finger finger = gesture.Fingers[0];

            if (gesture.Phase == ContinuousGesturePhase.Started)
            {
                if (gesture.Selection == null || gesture.Selection.transform.parent.GetComponent<SourceBoard>() == null) return;

                _dragObject = gesture.Selection;
                _dragStartPosition = _dragObject.transform.position;
                _dragObjectZ = _dragObject.transform.position.z;
                _dragFingerIndex = finger.Index;
                _movedOnTarget = false;

                dispatcher.Dispatch(ViewEventType.DragIsBegan);
            }
            else if (finger.Index == _dragFingerIndex && _dragObject != null)  // gesture in progress, make sure that this event comes from the finger that is dragging our dragObject
            {
                if (gesture.Phase == ContinuousGesturePhase.Updated)
                {
                    var pos = gesture.Position.GetWorldPos();
                    _dragObject.transform.position = new Vector3(pos.x, pos.y, _dragObjectZ);
                }
                else
                {
                    dispatcher.Dispatch(ViewEventType.DragIsFinished);

                    if (!_movedOnTarget) MoveObjectBack();
                    _dragFingerIndex = -1;
                    _dragObject = null;
                }
            }
        }

        private void MoveObjectBack()
        {
            var c = _dragObject.GetComponentInChildren<SphereCollider>();
            c.enabled = false;

            var param = new TweenParms();
            param.Prop("position", _dragStartPosition);
            param.OnComplete(() => c.enabled = true);

            HOTween.To(_dragObject.transform, MOVE_DURATION, param);
        }

        public void MoveDragObjectOnTarget(Vector3 targetPos)
        {
            if (_dragObject == null) return;
            
            _movedOnTarget = true;

            _dragObject.GetComponent<SphereCollider>().enabled = false;

            HOTween.To(_dragObject.transform, MOVE_DURATION, "position", targetPos);
        }

    }


}