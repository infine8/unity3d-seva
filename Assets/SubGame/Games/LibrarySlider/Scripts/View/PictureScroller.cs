using System;
using System.Collections.Generic;
using Holoville.HOTween;
using UniKid.SubGame.Games.LibrarySlider.Model;
using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System.Linq;

namespace UniKid.SubGame.Games.LibrarySlider.View
{
    public sealed class PictureScroller : EventView
    {
        [Inject]
        public LibrarySliderCore LibrarySliderCore { get; set; }

        [SerializeField] private GameObject _potContainer;

        [SerializeField] private float _showScaleFactor = 1.2f;
        [SerializeField] private tk2dSprite _picture0;
        [SerializeField] private tk2dSprite _picture1;
        [SerializeField] private tk2dSprite _picture2;


        public float MoveDurationTime = 1.0f;

        private Vector3 _pictureNormalScale = new Vector3(1.0f, 1.0f, 1.0f);
        
        private readonly tk2dSprite[] _pictureArray = new tk2dSprite[3]; // the second pic is current, the first - left, the third - right

        private Vector3 _leftPos;
        private Vector3 _centerPos;
        private Vector3 _rightPos;

        private int _currentPictureIndex = 0;

        private List<LibrarySliderStage.Picture> _pictureList;
        private List<Pot> _potList; 

        public void Init()
        {
            if (LibrarySliderCore.CurrentStage.PictureArray.Length < 3) throw new Exception("Not enough pictures in the list");

            _pictureArray[0] = _picture0;
            _pictureArray[1] = _picture1;
            _pictureArray[2] = _picture2;

            _leftPos = _picture0.transform.localPosition;
            _centerPos = _picture1.transform.localPosition;
            _rightPos = _picture2.transform.localPosition;

            _pictureList = LibrarySliderCore.CurrentStage.PictureArray.ToList();

            _currentPictureIndex = 0;

            UpdatePictureSprites();
            
            _potList = new List<Pot>(_potContainer.GetComponentsInChildren<Pot>());
            _potList.Sort((obj1, obj2) => obj1.Index - obj2.Index);

            UpdatePotChars(true);
        }

        public void MoveRight()
        {
            Move(true);
        }

        public void MoveLeft()
        {
            Move(false);
        }

        private void Move(bool toRight)
        {
            NormalizePictureScale(true);

            HOTween.Complete();

            foreach (var pic in _pictureArray) pic.gameObject.SetActive(true);

            var pic0NewPos = toRight ? _centerPos : _rightPos;
            var pic1NewPos = toRight ? _rightPos : _leftPos;
            var pic2NewPos = toRight ? _leftPos : _centerPos;

            HOTween.To(_pictureArray[0].transform, MoveDurationTime, "localPosition", pic0NewPos);
            HOTween.To(_pictureArray[1].transform, MoveDurationTime, "localPosition", pic1NewPos);
            HOTween.To(_pictureArray[2].transform, MoveDurationTime, "localPosition", pic2NewPos);

            if (Vector3.Distance(pic0NewPos, _rightPos) < 1) _pictureArray[0].gameObject.SetActive(false);
            if (Vector3.Distance(pic2NewPos, _leftPos) < 1) _pictureArray[2].gameObject.SetActive(false);

            var currentPictureArray = new tk2dSprite[_pictureArray.Length];
            for (var i = 0; i < _pictureArray.Length; i++) currentPictureArray[i] = _pictureArray[i];
            
            _pictureArray[0] = toRight ? currentPictureArray[2] : currentPictureArray[1];
            _pictureArray[1] = toRight ? currentPictureArray[0] : currentPictureArray[2];
            _pictureArray[2] = toRight ? currentPictureArray[1] : currentPictureArray[0];


            _currentPictureIndex = _currentPictureIndex + (toRight ? -1 : 1);
            if (_currentPictureIndex < 0) _currentPictureIndex = _pictureList.Count - 1;
            if (_currentPictureIndex == _pictureList.Count) _currentPictureIndex = 0;
            

            var param = new TweenParms();

            param.Prop("scale", _pictureNormalScale * _showScaleFactor);
            param.OnComplete(() => { HOTween.To(_pictureArray[1], MoveDurationTime / 2, "scale", _pictureNormalScale); NormalizePictureScale(false); });

            HOTween.To(_pictureArray[1], MoveDurationTime / 2, param);

            UpdatePictureSprites();
            UpdatePotChars(false);
        }

        private void NormalizePictureScale(bool isAll)
        {
            _pictureArray[0].scale = _pictureNormalScale;
            _pictureArray[2].scale = _pictureNormalScale;

            if (isAll) _pictureArray[1].scale = _pictureNormalScale;
        }

        private void UpdatePictureSprites()
        {
            for (var i = -1; i < _pictureArray.Length - 1; i++)
            {
                var picIndex = _currentPictureIndex + i;

                if (picIndex < 0) picIndex = _pictureList.Count - 1;
                if (picIndex == _pictureList.Count) picIndex = 0;

                _pictureArray[i + 1].SetSprite(Utils.GetCharSpriteName(LibrarySliderCore.CurrentLevel.PictureLibraryNameSequence, _pictureList[picIndex].CharName));
            }
        }

        private void UpdatePotChars(bool isInitiazation)
        {
            var word = _pictureList[_currentPictureIndex].DisplayName;

            var initOffset = (int) ((_potList.Count - word.Length)/2);
            var offset = initOffset;

            for (var i = 0; i < _potList.Count; i++)
            {
                offset--;

                string charName = null;

                if (offset < 0 && word.Length > i - initOffset) charName = word[i - initOffset].ToString();
                
                _potList[i].UpdateChar(LibrarySliderCore.CurrentLevel.LangLibraryNameSequence, charName, MoveDurationTime / 2, isInitiazation);
            }
        }
    }
}