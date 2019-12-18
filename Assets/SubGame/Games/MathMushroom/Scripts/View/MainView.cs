using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;

namespace UniKid.SubGame.Games.MathMushroom.View
{
    public sealed class MainView : MonoBehaviour
    {
        public Bug Bug;
        public Transform BugSpawnPoint;
        public Transform SwarmPoint;
        public List<Swarm> SwarmList;
        public tk2dTextMesh BugCountText;


        void OnSwipe(SwipeGesture gesture)
        {
            if (gesture.Direction == FingerGestures.SwipeDirection.Right) Move(true);
            if (gesture.Direction == FingerGestures.SwipeDirection.Left) Move(false);
        }


        private void Move(bool toRight)
        {
            var activeBugCount = Digit.ActiveBugList.Count;

            var nextBugCount = activeBugCount + (toRight ? -1 : 1);

            var maxBugCount = Digit.DigitList.FindMax(x => x.BugCount);

            if (nextBugCount < 1) nextBugCount = maxBugCount;
            if (nextBugCount > maxBugCount) nextBugCount = 1;

            var digit = Digit.DigitList.FirstOrDefault(x => x.BugCount == nextBugCount);

            digit.OnDigitClick();
        }
    }
}