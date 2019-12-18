using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System.Linq;

namespace UniKid.SubGame.Games.MathMushroom.View
{
    public sealed class Digit : MonoBehaviour
    {
        private static float FLYING_DURATION = 5.0f;
        private static float FLYING_AWAY_DURATION = 2.0f;
        private static int FLYING_CHECKPOINTS = 5;
        
        public int BugCount;

        public static List<Bug> ActiveBugList = new List<Bug>();
        
        public static List<Digit> DigitList;

        private static MainView _mv;

        private tk2dTextMesh _digitText;

        private void Start()
        {
            _digitText = GetComponent<tk2dTextMesh>();

            var b = GetComponent<tk2dButton>();

            b.targetObject = gameObject;

            if (_mv == null) _mv = (MainView) FindObjectOfType(typeof (MainView));

            if( DigitList == null) DigitList = new List<Digit>(transform.parent.GetComponentsInChildren<Digit>());
        }


        public void OnDigitClick()
        {

            var swarmOrig = _mv.SwarmList.FirstOrDefault(x => x.name.EndsWith(BugCount.ToString()));

            var swarm = (Instantiate(swarmOrig.gameObject) as GameObject).GetComponent<Swarm>();

            swarm.transform.parent = _mv.SwarmPoint;
            
            swarm.Init();

            var isDigitLess = false;

            if (BugCount == ActiveBugList.Count)
            {
                for (var i = 0; i < ActiveBugList.Count; i++) StartFlyingToSwarm(ActiveBugList[i], swarm.BugTransformList[i]);
            }


            if (BugCount > ActiveBugList.Count)
            {
                for (var i = 0; i < swarm.BugTransformList.Count; i++)
                {
                    var bugSwarmTransform = swarm.BugTransformList[i];

                    if (ActiveBugList.Count > i)
                    {
                        FixSwarmPosition(ActiveBugList[i], bugSwarmTransform);
                        continue;
                    }

                    var bug = PoolManager.Spawn(_mv.Bug.gameObject, _mv.BugSpawnPoint).GetComponent<Bug>();

                    var pos = bug.transform.position;

                    bug.transform.position = new Vector3(pos.x, pos.y, bugSwarmTransform.position.z);

                    StartFlyingToSwarm(bug, bugSwarmTransform);

                    ActiveBugList.Add(bug);
                }
            }
            else if (BugCount != ActiveBugList.Count)
            {
                for (var i = 0; i < swarm.BugTransformList.Count; i++) FixSwarmPosition(ActiveBugList[i], swarm.BugTransformList[i]);

                var bugList = new List<Bug>();
                ActiveBugList.ForEach(bugList.Add);

                for (var i = swarm.BugTransformList.Count; i < bugList.Count; i++)
                {
                    StartFlyingAway(bugList[i]);
                    ActiveBugList.Remove(bugList[i]);
                }

                isDigitLess = true;
            }

            Destroy(swarm);

            StartCoroutine(OnStartFlying(isDigitLess ? FLYING_AWAY_DURATION : FLYING_DURATION));
        }



        private void StartFlyingToSwarm(Bug bug, Transform endPoint)
        {
            var path = new List<Vector3>();

            for (var i = 0; i < FLYING_CHECKPOINTS; i++)
            {
                path.Add(new Vector3((Random.Range(-Screen.width / 2, Screen.width / 2)) / Camera.main.orthographicSize, (Random.Range(-Screen.height/2, Screen.height/2) - _mv.BugSpawnPoint.position.y) / Camera.main.orthographicSize, endPoint.position.z));
            }

            path.Add(endPoint.position);

            var param = new TweenParms();

            param.Prop("position", new PlugVector3Path(path.ToArray()));
            param.OnComplete(SetBugCountText);

            HOTween.To(bug.transform, FLYING_DURATION, param);
        }

        private void FixSwarmPosition(Bug bug, Transform endPoint)
        {
            HOTween.To(bug.transform, FLYING_DURATION, "position", endPoint.position);
        }

        private void StartFlyingAway(Bug bug)
        {
            var param = new TweenParms();

            var endPoint = new Vector3(_mv.BugSpawnPoint.position.x, _mv.BugSpawnPoint.position.y, bug.transform.position.z);

            param.Prop("position", endPoint);
            param.OnComplete(() => { PoolManager.Despawn(bug.gameObject); SetBugCountText(); });

            HOTween.To(bug.transform, FLYING_AWAY_DURATION, param);
        }

        private IEnumerator OnStartFlying(float duration)
        {
            DigitList.ForEach(x => x.gameObject.collider.enabled = false);

            yield return new WaitForSeconds(duration);

            DigitList.ForEach(x => x.gameObject.collider.enabled = true);
        }

        private void SetBugCountText()
        {
            _mv.BugCountText.text = ActiveBugList.Count.ToString();
            _mv.BugCountText.Commit();

            DigitList.ForEach(x =>
                                  {
                                      var text = x.GetComponent<tk2dTextMesh>();
                                      text.color = Color.white;
                                      text.Commit();
                                  });

            _digitText.color = Color.red;
            _digitText.Commit();
        }
    }
}