using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TextureTrailItem : MonoBehaviour
{

    [SerializeField]
    private bool _isHead;

    [System.NonSerialized]
    public List<TextureTrailWaypoint> WaypointList;

    [System.NonSerialized] public TextureTrailItem PreviousItem;

    private int _currentWpIndex = 0;
    private Transform _transform;

    // Use this for initialization
    void Start()
    {
        _transform = transform;

        //if (PreviousItem != null)
        //{
        //    var sj = GetComponent<FixedJoint>();
        //    sj.connectedBody = PreviousItem.rigidbody;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && WaypointList != null && _currentWpIndex < WaypointList.Count)
        {
            if (_isHead)
            {
                var wp0 = WaypointList[_currentWpIndex].transform.position;
                var wp1 = WaypointList[_currentWpIndex + 1].transform.position;

                //wp1 - wp0 - direction of waypoints
                //Input.mousePosition - wp0 - mouse position about start point
                var project = Vector3.Project(Input.mousePosition - wp0, wp1 - wp0);

                _transform.position = wp0;

                _transform.Translate(project);

                if (Vector3.Distance(_transform.position, wp1) < 0.01f) _currentWpIndex++;
            }
            

        }

        if (!_isHead)
        {

            _transform.position = PreviousItem.transform.position + new Vector3(0, 10, 0);
        }
    }
}
