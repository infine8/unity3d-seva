using System.Collections.Generic;
using UniKid.SubGame.Games.LibrarySlider.Model;
using UniKid.SubGame.Model;
using UniKid.SubGame.View;
using UnityEngine;
using System.Collections;

namespace UniKid.SubGame.Games.LibrarySlider.View
{
    public sealed class MainView : SubGameMainViewBase
    {
        public PictureScroller PictureScroller;

        [Inject]
        public LibrarySliderCore LibrarySliderCore { get; set; }

        public override SubGameCoreBase SubGameCoreBase
        {
            get { return LibrarySliderCore; }
        }

        protected override void Start()
        {
            base.Start();

            LoadStage();
        }

        private void ButtonRightClick()
        {
            PictureScroller.MoveRight();
        }

        private void ButtonLeftClick()
        {
            PictureScroller.MoveLeft();
        }

        void OnSwipe(SwipeGesture gesture)
        {
            if (gesture.Direction == FingerGestures.SwipeDirection.Right) PictureScroller.MoveRight();
            if (gesture.Direction == FingerGestures.SwipeDirection.Left) PictureScroller.MoveLeft();
        }

        public override void LoadStage()
        {
            PictureScroller.Init();
        }
		
		private void ExitGame()
        {
            dispatcher.Dispatch(SubGameViewEvent.ExitToMainMenu);
        }
    }
}