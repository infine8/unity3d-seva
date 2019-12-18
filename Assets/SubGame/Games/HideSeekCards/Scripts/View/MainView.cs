using System;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using UniKid.SubGame.Games.HideSeekCards.Model;
using UniKid.SubGame.Model;
using UniKid.SubGame.View;
using UniKid.SubGame.View.Character;
using UnityEngine;

namespace UniKid.SubGame.Games.HideSeekCards.View
{
    public sealed class MainView : SubGameMainViewBase
    {
        [Inject]
        public HideSeekCardsCore HideSeekCardsCore { get; set; }

        [SerializeField] private float _timeToDestroyOldField = 2f;

        public List<Field> FieldList = new List<Field>();

        
        public override SubGameCoreBase SubGameCoreBase
        {
            get { return HideSeekCardsCore; }
        }

        protected override void Start()
        {
            base.Start();
            
            LoadStage();
        }


        public override void LoadStage()
        {
            var loadStageAct = new Action(() =>
            {

                if (HideSeekCardsCore.CurrentStage == null) return;

                var currentField = FieldList.Find(x => x.name.Equals(HideSeekCardsCore.CurrentStage.Type));

                var go = (GameObject)Instantiate(currentField.gameObject);

                go.transform.parent = transform;
            });

            var oldField = GetComponentInChildren<Field>();

            if(oldField == null)
            {
                loadStageAct();
            }
            else
            {
                StartCoroutine(DestroyOldField(oldField, loadStageAct));
            }
        }

        private IEnumerator DestroyOldField(Field field, Action onDestroy)
        {
            var baloon = GetComponentInChildren<DreamingBaloon>();
            if (baloon != null) baloon.Hide();

            var cardList = field.GetComponentsInChildren<Card>();
            
            foreach (var card in cardList)
            {
                HOTween.To(card.transform, _timeToDestroyOldField, "localScale", Vector3.zero);
            }

            yield return new WaitForSeconds(_timeToDestroyOldField);

            Destroy(field.gameObject);

            onDestroy();
        }

		
		private void ExitGame()
        {
            dispatcher.Dispatch(SubGameViewEvent.ExitToMainMenu);
        }

    }
}