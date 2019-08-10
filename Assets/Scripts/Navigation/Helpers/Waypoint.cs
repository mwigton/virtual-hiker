using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour
{

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "WaypointMarker.jpg",false);
    }

}
