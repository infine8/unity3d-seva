using System.Collections.Generic;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.TexturePaintGame.View
{
    public class FootTrail : EventView
    {
        [SerializeField]
        private List<FootTrailItem> _trailItemList = new List<FootTrailItem>();
        [SerializeField]
        private float _trailItemSpawnPeriod = 2;
        [SerializeField]
        private float _disapearDuration = 2.0f;

        private float _nextSpawnTime;
        private int _currentTrailItemIndex;

        private void Update()
        {
            if (Time.realtimeSinceStartup > _nextSpawnTime) SpawnTrailItem();
        }

        private void SpawnTrailItem()
        {
            _nextSpawnTime = GetNextWaterBubbleSpawnTime();

            if (_currentTrailItemIndex == _trailItemList.Count) _currentTrailItemIndex = 0;
            var trailItem = _trailItemList[_currentTrailItemIndex++];

            var go = PoolManager.Spawn(trailItem.gameObject, transform.parent);

            go.GetComponent<FootTrailItem>().Init(_disapearDuration);

            var pos = transform.localPosition;
            go.transform.localPosition = new Vector3(pos.x, pos.y, trailItem.transform.localPosition.z);
            go.transform.localRotation = transform.localRotation;
        }


        private float GetNextWaterBubbleSpawnTime()
        {
            return Time.realtimeSinceStartup + _trailItemSpawnPeriod;
        }
    }
}