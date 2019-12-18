using System;
using System.Collections;
using System.Collections.Generic;
using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.SeaGuessing.Model;
using UniKid.SubGame.View;
using UniKid.SubGame.View.Character;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UniKid.SubGame.Games.SeaGuessing.View
{
    public sealed class MainView : SubGameMainViewBase
    {
        public Transform WaterBubbleSpawnPoint;
        public Transform CharGroupSpawnPoint;
        public GameObject WaterBubbleObject;
        public GameObject ScoreItemFull;
        public tk2dSprite GuessingChar;
        public List<BubbleCharGroup> BubbleCharGroupList;
        public List<ScoreItem> ScoreItemList;
        public float ScoreItemMoveDuration = 3f;
        public float ShowDoingNothingAnimationDuration = 5f;
        public float ShowNextGroupAfterDoingNothingDuration = 10f;
        public DreamingBaloon Baloon;
		
        [Inject]
        public SeaGuessingCore SeaGuessingCore { get; set; }

        private float _nextSpawnWaterBubbleTime;

        private readonly string[] _bubbleAnimations = new[] { "WaterBubble1", "WaterBubble2", "WaterBubble3" };
        
        private BubbleCharGroup _lastSpawnedObject;


        public override SubGame.Model.SubGameCoreBase SubGameCoreBase
        {
            get { return SeaGuessingCore; }
        }

        public override float SecondsBeforeLoadNewLevel
        {
            get
            {
                return 2f;
            }
        }

        protected override void Start()
        {
            base.Start();

            SpawnWaterBubble();

            LoadStage();
            
            AudioController.PlayMusic("sea_background");

            AudioController.PlayAfter("turtle_help", "find_letter");
        }

        protected override void Update()
        {
            base.Update();

            if (Time.realtimeSinceStartup > _nextSpawnWaterBubbleTime) SpawnWaterBubble();
        }

        private void SpawnWaterBubble()
        {
            _nextSpawnWaterBubbleTime = GetNextWaterBubbleSpawnTime();

            var go = PoolManager.Spawn(WaterBubbleObject, WaterBubbleSpawnPoint);

            go.animation.Play(_bubbleAnimations[Random.Range(0, _bubbleAnimations.Length)]);
        }

        private float GetNextWaterBubbleSpawnTime()
        {
            return Time.realtimeSinceStartup + Random.Range(1, 3);
        }

        public override void LoadStage()
        {
            if (SeaGuessingCore.CurrentStage == null) return;

            StartCoroutine(ShowBaloon());

            var charGroup = BubbleCharGroupList.Find(x => x.name.EndsWith(SeaGuessingCore.CurrentStage.PossibleCharSequence.SplitSequence().Count.ToString()));

            _lastSpawnedObject = PoolManager.Spawn(charGroup.gameObject, CharGroupSpawnPoint).GetComponent<BubbleCharGroup>();
        }

        public void DespawnLastGroup()
        {
            Baloon.Hide();
            StartCoroutine(_lastSpawnedObject.DespawnGroup(null));
        }
		
        private IEnumerator ShowBaloon()
        {
            yield return new WaitForSeconds(BubbleChar.SCALE_DURATION);

            Baloon.Show();
        }

        private void ExitGame()
        {
            dispatcher.Dispatch(SubGameViewEvent.ExitToMainMenu); 
        }

    }
}