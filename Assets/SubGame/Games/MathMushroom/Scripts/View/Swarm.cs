using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace UniKid.SubGame.Games.MathMushroom.View
{
    public sealed class Swarm : MonoBehaviour
    {
        public List<Transform> BugTransformList { get; set; }
 
        public void Init()
        {
            transform.localPosition = Vector3.zero;

            BugTransformList = new List<Transform>();

            var bugList = new List<Bug>(GetComponentsInChildren<Bug>(true));
            bugList.Sort((item1, item2) => item1.SwarmPositionIndex - item2.SwarmPositionIndex);

            bugList.ForEach(x => { BugTransformList.Add(x.transform); x.gameObject.SetActive(false); });
        }
    }
}