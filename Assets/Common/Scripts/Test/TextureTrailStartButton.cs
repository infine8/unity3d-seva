using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;

public class TextureTrailStartButton : MonoBehaviour
{
    [SerializeField] private TextureTrail _textureTrail;
    [SerializeField] private Transform _spawnHeadPoint;
    [SerializeField] private GameObject _waypoint;
    [SerializeField] private TextureTrailRope _ropeObj;

    private TextureTrailRope _rope;
    private List<TextureTrailWaypoint> _waypointList;
    private TextureTrailItem _lastTrailItem;
    private int _currentWpIndex = 0;
    private TouchScript.Gestures.TapGesture _tapGesture;
    private GameObject _cursorPositionObject;
    private float _lastRopeLength;
    private List<float> _ropeLenghtConrolPointList = new List<float>(); 
    private List<Vector3> _tapPositionList = new List<Vector3>(); 

    void Start()
    {
        _waypointList = new List<TextureTrailWaypoint>(_waypoint.GetComponentsInChildren<TextureTrailWaypoint>());

        _cursorPositionObject = new GameObject("CursorPosition");

        _tapGesture = (TouchScript.Gestures.TapGesture)FindObjectOfType(typeof(TouchScript.Gestures.TapGesture));

        _rope = (TextureTrailRope)FindObjectOfType(typeof(TextureTrailRope));

        _rope.StartPoint.localPosition = _rope.EndPoint.localPosition = _waypointList[0].transform.position;

        _tapPositionList.Add(_rope.EndPoint.localPosition);
    }
    

    void FixedUpdate()
    {
        if (_tapGesture.State == TouchScript.Gestures.Gesture.GestureState.Possible && _tapPositionList.Count > 0)
        {
            var lastPos = _tapPositionList[_tapPositionList.Count - 1];
            _tapPositionList.RemoveAt(_tapPositionList.Count - 1);

            MoveRope(lastPos);
        }
        else if (_tapGesture.State == TouchScript.Gestures.Gesture.GestureState.Changed)
        {
            if (_tapPositionList.Count == 0) _tapPositionList.Add(_waypointList[0].transform.position);
            _tapPositionList.Add(_tapGesture.ScreenPosition);
            MoveRope(_tapGesture.ScreenPosition);
        }

    }

    private void MoveRope(Vector3 inputPosition)
    {
        if (float.IsNaN(inputPosition.x) || float.IsNaN(inputPosition.y) || float.IsNaN(inputPosition.z)) return;
        if (_rope == null || _currentWpIndex == _waypointList.Count - 1) return;
        
        var wp0 = _waypointList[_currentWpIndex].transform.position;
        var wp1 = _waypointList[_currentWpIndex + 1].transform.position;
        var waypointDirection = wp1 - wp0;

        //wp1 - wp0 - direction of waypoints
        //Input.mousePosition - wp0 - mouse position about start point
        var project = Vector3.Project(inputPosition - wp0, waypointDirection);

        _cursorPositionObject.transform.localPosition = wp0;

        _cursorPositionObject.transform.Translate(project);

        var cursorDicrection = _cursorPositionObject.transform.localPosition - wp0;

        if(Vector3.Angle(cursorDicrection, waypointDirection) < 0.01f) // if vectors is co-direction
            if(cursorDicrection.sqrMagnitude < waypointDirection.sqrMagnitude) // if cursor direction vector is less than waypoint direction vector
                UpdateRopePosition(wp1);
    }


    private void UpdateRopePosition(Vector3 wp1)
    {
        _rope.EndPoint.localPosition = _cursorPositionObject.transform.localPosition;
        _rope.Rope.ApplyRopeSettings();

        var ropeLegth = _rope.Rope.RopeLength;

        if (_lastRopeLength < ropeLegth && Vector3.Distance(_rope.EndPoint.position, wp1) < 10f)
        {
            _rope.Rope.ControlPoints.Add(wp1);
            _currentWpIndex++;

            _ropeLenghtConrolPointList.Add(ropeLegth);
        }

        if (_ropeLenghtConrolPointList.Count > 0 && _lastRopeLength > ropeLegth)
        {

            if (ropeLegth - _ropeLenghtConrolPointList[_ropeLenghtConrolPointList.Count - 1] < 30)
            {
                _rope.Rope.ControlPoints.RemoveAt(_rope.Rope.ControlPoints.Count - 1);
                _currentWpIndex--;
                _ropeLenghtConrolPointList.RemoveAt(_ropeLenghtConrolPointList.Count - 1);
            }
        }

        _lastRopeLength = ropeLegth;
    }


}
