using UnityEngine;

namespace UniKid.SubGame.Games.TexturePaintGame.View
{
    public class Waypoint : MonoBehaviour
    {
        public Waypoint NextWaypoint = null;
        public bool IsConnectedWithNext = true;
        public bool IsControlPoint = true;
        public bool IsStartOfNewPart = false;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (NextWaypoint != null && IsConnectedWithNext) Gizmos.DrawLine(transform.position, NextWaypoint.transform.position);
        }
    }


}

