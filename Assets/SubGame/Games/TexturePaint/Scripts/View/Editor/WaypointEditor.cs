using UniKid.SubGame.Games.TexturePaintGame.View;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor
{
    private readonly string WAYPOINT_PREFIX = "wp";

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();
        if (GUILayout.Button("Create Next Waypoint"))
        {
            var wp = (Waypoint) serializedObject.targetObject;
            int wpIndex;
            if (!int.TryParse(wp.name.Replace(WAYPOINT_PREFIX, string.Empty), out wpIndex)) wpIndex = 0;

            var nextWp = new GameObject(string.Format("{0}{1}", WAYPOINT_PREFIX, (wpIndex+1).ToString("00")));
            nextWp.transform.parent = wp.transform.parent;
            nextWp.AddComponent<Waypoint>();
            nextWp.transform.localPosition = wp.transform.localPosition;

            wp.NextWaypoint = nextWp.GetComponent<Waypoint>();
        }
    }
}
