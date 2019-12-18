using UnityEngine;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.SeaGuessing.View
{
    public class WaterView : EventView
    {
        void OnTriggerExit(Collider other)
        {
            Debug.Log(other.name);
            if(other.gameObject.GetComponent<PoolObject>() != null) PoolManager.Despawn(other.gameObject);
        }
    }
}