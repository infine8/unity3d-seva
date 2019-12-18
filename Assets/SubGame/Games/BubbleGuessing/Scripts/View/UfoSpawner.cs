using UniKid.SubGame.Games.BubbleGuessing.Model;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.BubbleGuessing.View
{
    public sealed class UfoSpawner : EventView
    {
        [Inject]
        public BubbleGuessingCore BubbleGuessingCore { get; set; }

        [SerializeField] private GameObject _char;
        [SerializeField] private Vector2 _randomHeightRange;

        private float _nextSpawnTime;

        private bool _isSpawn;

        void Update()
        {
            if (Time.realtimeSinceStartup > _nextSpawnTime && _isSpawn) SpawnChar();
        }

        private void SpawnChar()
        {
            if (BubbleGuessingCore.CurrentStage == null) return;

            _nextSpawnTime = GetNextSpawnTime();
            
            var ss = BubbleGuessingCore.CurrentLevel.SpawnSettings;

            if (UfoChar.Counter == Random.Range(ss.MaxCharOnScreenRange.From, ss.MaxCharOnScreenRange.To)) return;

            var go = PoolManager.Spawn(_char, transform);
            var pos = go.transform.localPosition;
            go.transform.localPosition = new Vector3(pos.x, pos.y + Random.Range(_randomHeightRange.x, _randomHeightRange.y), _char.transform.localPosition.z);
        }

        private float GetNextSpawnTime()
        {
            var ss = BubbleGuessingCore.CurrentLevel.SpawnSettings;

            return Time.realtimeSinceStartup + Random.Range(ss.SpawnPeriodRange.From, ss.SpawnPeriodRange.To);
        }

        public void StopSpawn()
        {
            _isSpawn = false;
        }

        public void StartSwawn()
        {
            _isSpawn = true;
        }
    }
}