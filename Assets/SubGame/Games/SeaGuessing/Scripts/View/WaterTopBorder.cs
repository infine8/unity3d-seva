using UnityEngine;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.SeaGuessing.View
{
    public sealed class WaterTopBorder : EventView
    {
        public enum EventType
        {
            BubbleGroupReachedTopBorder
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PoolObject>() != null) PoolManager.Despawn(other.gameObject);

            if (other.CompareTag(Const.TAG_SeaGuessing_BubbleCharGroup))  dispatcher.Dispatch(EventType.BubbleGroupReachedTopBorder);
        }
    }
}