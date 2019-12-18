using System;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using UniKid.SubGame.Games.HideSeekCards.View;
using UniKid.SubGame.Games.TexturePaintGame.Model;
using UniKid.SubGame.Model;
using UniKid.SubGame.View;
using UniKid.SubGame.View.Character;
using UnityEngine;

namespace UniKid.SubGame.Games.TexturePaintGame.View
{
    public sealed class MainView : SubGameMainViewBase
    {
        [SerializeField] private Transform _root;
        [SerializeField] private Brush _brush;

        public List<PaintChar> PaintCharList;
            
        [Inject]
        public TexturePaintCore TexturePaintCore { get; set; }

        [SerializeField]
        private float _timeToDestroyOldField = 0.5f;

        public override SubGameCoreBase SubGameCoreBase
        {
            get { return TexturePaintCore; }
        }

        protected override void Awake()
        {
            base.Awake();

            LoadStage();
        }

        public override void LoadStage()
        {
            var loadStageAct = new Action(() =>
            {
                if (TexturePaintCore.CurrentStage == null) return;


                var paintCharOrig = PaintCharList.Find(x => x.name.Equals(TexturePaintCore.CurrentStage.PaintCharName));
                var pos = paintCharOrig.transform.localPosition;

                var go = (GameObject)Instantiate(paintCharOrig.gameObject);
                go.transform.parent = _root;
                go.transform.localPosition = pos;

                var paintChar = go.GetComponent<PaintChar>();
                paintChar.Init();
                _brush.Init(paintChar);

            });

            var oldPaintChar = GetComponentInChildren<PaintChar>();

            if (oldPaintChar == null)
            {
                loadStageAct();
            }
            else
            {
                StartCoroutine(DestroyOldField(oldPaintChar, loadStageAct));
            }
        }


        private IEnumerator DestroyOldField(PaintChar paintChar, Action onDestroy)
        {
            _brush.DestroyPaint(_timeToDestroyOldField);
            HOTween.To(paintChar.gameObject.transform, _timeToDestroyOldField, "localScale", Vector3.zero);

            yield return new WaitForSeconds(_timeToDestroyOldField);

            Destroy(paintChar.gameObject);

            onDestroy();
        }
		
		private void ExitGame()
        {
            dispatcher.Dispatch(SubGameViewEvent.ExitToMainMenu);
        }
    }

}

