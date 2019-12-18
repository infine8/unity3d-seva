using System;
using System.Collections.Generic;
using UniKid.SubGame.Games.WordConstructor02.Model;
using UniKid.SubGame.Model;
using UniKid.SubGame.View;
using UnityEngine;
using System.Collections;

namespace UniKid.SubGame.Games.WordConstructor02.View
{
    public sealed class MainView : SubGameMainViewBase
    {
        [Inject]
        public WordConstructor02Core WordConstructor02Core { get; set; }

        [SerializeField]
        private tk2dSprite _guessingPicture;
        [SerializeField]
        private List<ScrollField> _scrollFieldList;
        [SerializeField]
        private List<tk2dTextMesh> _textMeshList;
        [SerializeField]
        private Transform _scrollFieldParent;
        [SerializeField]
        private Ufo _ufo;

        public override SubGameCoreBase SubGameCoreBase
        {
            get { return WordConstructor02Core; }
        }

        private ScrollField _currentScrollField;
        private int _dragFingerIndex = -1;
        private Scroll _dragScroll;

        protected override void Start()
        {
            base.Start();

            LoadStage();
        }

        public override void LoadStage()
        {
            var loadLevelAct = new Action(() =>
            {
                var syllableList = WordConstructor02Core.CurrentStage.SyllableSequence.SplitSequence();

                var scrollFieldOrig = _scrollFieldList.Find(x => x.name.EndsWith(syllableList.Count.ToString()));

                if (scrollFieldOrig == null) throw new Exception("Scroll field is not found with length " + syllableList.Count);

                _ufo.ShowPicture(_currentScrollField == null);

                _currentScrollField = PoolManager.Spawn(scrollFieldOrig.gameObject, _scrollFieldParent).GetComponent<ScrollField>();

                _currentScrollField.transform.localPosition = scrollFieldOrig.transform.localPosition;

                _currentScrollField.Init(_textMeshList);

                _guessingPicture.SetSprite(Utils.GetCharSpriteName(WordConstructor02Core.CurrentLevel.PictureLibraryNameSequence, WordConstructor02Core.CurrentStage.CharName));


            });

            if (_currentScrollField == null)
            {
                loadLevelAct();
            }
            else
            {
                DespawnCurrentField(loadLevelAct);
            }

        }


        private void OnSwipe(SwipeGesture gesture)
        {
            if (_currentScrollField == null) return;

            RaycastHit hit;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(gesture.StartPosition), out hit)) return;

            var touchedScroll = _currentScrollField.ScrollList.Find(x => x.tag.Equals(hit.transform.tag));
            if (touchedScroll == null) return;

            if (gesture.Direction == FingerGestures.SwipeDirection.Up) touchedScroll.ScrollUp(gesture.Velocity);
            if (gesture.Direction == FingerGestures.SwipeDirection.Down) touchedScroll.ScrollDown(gesture.Velocity);
        }


        private void OnTap(TapGesture gesture)
        {
            if (gesture.Selection == null) return;

            var scroll = gesture.Selection.GetComponent<Scroll>();

            if (scroll != null) scroll.StopScrolling();
        }

        private void OnDrag(DragGesture gesture)
        {
            if (gesture.Selection == null) return;

            FingerGestures.Finger finger = gesture.Fingers[0];

            if (gesture.Phase == ContinuousGesturePhase.Started)
            {
                _dragScroll = gesture.Selection.GetComponent<Scroll>();

                if (_dragScroll == null) return;

                _dragFingerIndex = finger.Index;
            }
            else if (finger.Index == _dragFingerIndex && _dragScroll != null)  // gesture in progress, make sure that this event comes from the finger that is dragging our dragObject
            {
                if (gesture.Phase == ContinuousGesturePhase.Updated)
                {
                    _dragScroll.DragScroll(gesture.StartPosition, gesture.Position);
                }
                else
                {
                    _dragScroll.StopScrolling();

                    _dragFingerIndex = -1;
                }
            }
        }

        private void DespawnCurrentField(Action onDespawn)
        {
            _ufo.HidePicture(onDespawn);
            _currentScrollField.Hide();
        }

        private void ExitGame()
        {
            dispatcher.Dispatch(SubGameViewEvent.ExitToMainMenu);
        }

    }
}