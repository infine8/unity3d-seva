using System;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System.Linq;

namespace UniKid.SubGame.Games.WordConstructor02.View
{
    public sealed class DragItemPosition
    {
        public bool? IsDrugUp { get; set; }

        public int CurrentPositionIndex { get; set; }
    }

    public sealed class Scroll : EventView
    {
        private static readonly float MIN_VELOCTIY = 400f;
        public static readonly int CURRENT_ITEM_INDEX = 3;
        public static readonly float MOVING_DURATION = 2.0f;
        public static readonly float STOP_DURATION = 0.3f;


        public enum AnimationType
        {
            ScrollShow,
            ScrollHide
        }

        public enum ViewEventType
        {
            CurrentItemDetected
        }

        public int Index;



        public ScrollItem CurrentItem { get; private set; }

        public List<ScrollItem> ItemList { get; private set; }

        public bool IsScrollingInProcess { get; private set; }

        private readonly List<Vector3> _initPositionList = new List<Vector3>();

        private readonly List<List<Vector3>> _targetItemPositionPathList = new List<List<Vector3>>();

        private readonly Dictionary<int, DragItemPosition> _latestDragItemPositionDict = new Dictionary<int, DragItemPosition>();

        private Vector2 _lastDragPos = Vector2.zero;


        public void Show()
        {
            ItemList = new List<ScrollItem>();
            _latestDragItemPositionDict.Clear();

            ItemList.AddRange(GetComponentsInChildren<ScrollItem>());

            ItemList.Sort((obj1, obj2) => obj1.Index - obj2.Index);

            ItemList.ForEach(x => _initPositionList.Add(x.transform.localPosition));

            for (var i = 0; i < ItemList.Count; i++) _latestDragItemPositionDict.Add(i, new DragItemPosition { CurrentPositionIndex = i });

            CurrentItem = ItemList[CURRENT_ITEM_INDEX];
            
            animation.Play(AnimationType.ScrollShow.ToString());
        }

        public void Hide()
        {
            animation.Play(AnimationType.ScrollHide.ToString());
        }

        public void ScrollUp(float velocity)
        {
            //Debug.Log("swipe - " + velocity);

            //ProcessScrolling(true, velocity);
        }

        public void ScrollDown(float velocity)
        {
            //Debug.Log("swipe - " + velocity);

            //ProcessScrolling(false, velocity);
        }

        public void StopScrolling()
        {
            if (!IsScrollingInProcess) return;

            ProcessScrolling(true, 0, -1);

            _lastDragPos = Vector2.zero;
        }



        public void DragScroll(Vector2 startPos, Vector2 currentPos)
        {
            IsScrollingInProcess = true;

            var delta = Mathf.Abs(currentPos.y - _lastDragPos.y) / 15;

            if (delta > 0.1 && _lastDragPos != Vector2.zero)
            {
                var dragUp = currentPos.y > _lastDragPos.y;


                for (var i = 0; i < ItemList.Count; i++)
                {
                    var item = ItemList[i];

                    var pos = item.transform.localPosition;

                    var currentPosIndex = GetCurrentPosIndex(pos);

                    var nextPosIndex = -1;

                    if (currentPosIndex > -1)
                    {
                        _latestDragItemPositionDict[i].CurrentPositionIndex = currentPosIndex;
                        _latestDragItemPositionDict[i].IsDrugUp = dragUp;
                        nextPosIndex = GetCorrectPositionIndex(currentPosIndex + (dragUp ? -1 : 1));
                        //ProcessScrolling(true, 0, 0);
                    }
                    else
                    {
                        currentPosIndex = _latestDragItemPositionDict[i].CurrentPositionIndex;

                        if (!_latestDragItemPositionDict[i].IsDrugUp.HasValue) _latestDragItemPositionDict[i].IsDrugUp = dragUp;

                        nextPosIndex = _latestDragItemPositionDict[i].IsDrugUp != dragUp ? 
                            currentPosIndex : GetCorrectPositionIndex(currentPosIndex + (dragUp ? -1 : 1));
                    }

                    var currentIndexPos = _initPositionList[currentPosIndex];

                    var y = pos.y;
                    var z = pos.z;

                    if (nextPosIndex <= 5)
                    {
                        y = pos.y + delta * (dragUp ? 1 : -1);
                        z = currentIndexPos.z;
                    }
                    if (nextPosIndex >= 6)
                    {
                        y = pos.y + delta * (dragUp ? -1 : 1);
                        z = currentIndexPos.z;
                    }

                    if (
                        ((nextPosIndex == 6 || nextPosIndex == 0) && !dragUp)
                        || ((nextPosIndex == 11 || nextPosIndex == 5) && dragUp)
                        )
                    {
                        y = currentIndexPos.y;

                        if (nextPosIndex == 6 || nextPosIndex == 11) z = pos.z + delta;
                        if (nextPosIndex == 0 || nextPosIndex == 5) z = pos.z - delta;
                    }



                    item.transform.localPosition = new Vector3(pos.x, y, z);

                }
            }


            _lastDragPos = currentPos;

        }

        private void ProcessScrolling(bool isUp, float velocity, float duration)
        {

            _targetItemPositionPathList.Clear();

            var itemToScrollCount = (int)(velocity / MIN_VELOCTIY);
            var currentPosIndex0 = -1;

            for (var i = 0; i < ItemList.Count; i++)
            {
                var pathList = new List<Vector3>();
                var currentPosIndex = -1;

                if (i == 0) currentPosIndex = currentPosIndex0 = GetNearestItemIndex(ItemList[i].transform.localPosition);
                else currentPosIndex = GetCorrectPositionIndex(currentPosIndex0 + i);

                if (itemToScrollCount > 0)
                {
                    for (var j = 0; j < itemToScrollCount; j++)
                    {
                        var targetPosIndex = GetTargetPosIndex(isUp, currentPosIndex, j);

                        pathList.Add(_initPositionList[targetPosIndex]);
                    }
                }
                else
                {
                    pathList.Add(_initPositionList[currentPosIndex]);
                }

                _targetItemPositionPathList.Add(pathList);
            }


            StartMooving(duration < 0 ? (itemToScrollCount > 0 ? MOVING_DURATION : STOP_DURATION) : duration);

        }

        private int GetTargetPosIndex(bool isUp, int itemIndex, int scrollItemIndex)
        {
            var targetPosIndex = itemIndex + (scrollItemIndex + 1) * (isUp ? -1 : 1);

            return GetCorrectPositionIndex(targetPosIndex);
        }

        private int GetCorrectPositionIndex(int posIndex)
        {
            if (posIndex < 0)
            {
                posIndex = ItemList.Count + posIndex % ItemList.Count;

                if (posIndex == ItemList.Count) posIndex = 0;
            }

            if (posIndex > ItemList.Count - 1) posIndex = posIndex % ItemList.Count;


            return posIndex;
        }

        private void StartMooving(float movingDuration)
        {
            for (var i = 0; i < ItemList.Count; i++)
            {
                if (_targetItemPositionPathList[i].Count < 1) continue;

                var param = new TweenParms();

                if (_targetItemPositionPathList[i].Count > 1)
                {
                    param.Prop("localPosition", new PlugVector3Path(_targetItemPositionPathList[i].ToArray()));
                }
                else
                {
                    param.Prop("localPosition", _targetItemPositionPathList[i][0]);
                }


                if (i == 0)
                {
                    HOTween.Kill();

                    param.OnStart(() => { IsScrollingInProcess = true; });
                    param.OnComplete(DetectCurrentItem);
                }


                HOTween.To(ItemList[i].transform, movingDuration, param);
            }
        }

        private void DetectCurrentItem()
        {
            if (HOTween.totTweens > 0) return;

            IsScrollingInProcess = false;

            CurrentItem = ItemList.FirstOrDefault(x => Vector3.Distance(x.transform.localPosition, _initPositionList[CURRENT_ITEM_INDEX]) < 1);

            if (CurrentItem == null) throw new Exception("Couldn't detect current item");

            dispatcher.Dispatch(ViewEventType.CurrentItemDetected);
        }

        private int GetNearestItemIndex(Vector3 pos)
        {
            var minDistance = _initPositionList.Min(x => Vector3.Distance(x, pos));

            return _initPositionList.IndexOf(_initPositionList.Find(x => Mathf.Abs(Vector3.Distance(x, pos) - minDistance) < 0.1));
        }

        private int GetCurrentPosIndex(Vector3 pos)
        {
            return _initPositionList.FindIndex(x => Mathf.Abs(Vector3.Distance(x, pos)) < 1.5);
        }
    }
}