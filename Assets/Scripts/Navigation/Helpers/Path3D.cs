using UnityEngine;
using System.Collections;
//using UnityEditor;

[ExecuteInEditMode]
public class Path3D : MonoBehaviour
{
    #region Variables
   
    public Transform[] waypoints;
    Transform[] waypointsBuffer;
    int waypointLengthBuffer;
    public bool canCheckWaypoints = true;
    public float height;
    public float oldHeight;
    public Color pathColor;

    #endregion

    void Update()
    {
        if (canCheckWaypoints)
        {
            //----------- Check if waypoint objects have changed -----------------
            waypointsBuffer = GetComponentsInChildren<Transform>();
            if (waypointsBuffer[0] == transform && waypointsBuffer.Length - 1 != waypointLengthBuffer)
            {
                UpdateWaypoints();
            }
        }
    }


    /// <summary>
    /// Adds a new waypoint to the path
    /// </summary>
    /// <param name="position"></param>
    public void AddWaypoint(Vector3 position)
    {
        GameObject newWaypoint = new GameObject("New Waypoint");
        newWaypoint.transform.position = position;
        newWaypoint.transform.parent = transform;
        newWaypoint.AddComponent<Waypoint>();
    }

    /// <summary>
    /// Updates the array of waypoints
    /// Updates names and height of the waypoints
    /// </summary>
    public void UpdateWaypoints()
    {
        Debug.Log("Updating waypoints");

        Transform[] tempHolder = new Transform[waypointsBuffer.Length - 1];
        for (int i = 0; i < tempHolder.Length; i++)
        {
            tempHolder[i] = waypointsBuffer[i + 1];
        }
        waypoints = tempHolder;
        waypointLengthBuffer = waypoints.Length;

        for (int i = 0; i < waypoints.Length; i++)
        {
            string num;
            if (waypoints.Length < 10) { num = "00" + i; }
            else if (waypoints.Length < 100) { num = "0" + i; }
            else { num = i.ToString(); }
            waypoints[i].name = "Waypoint " + num;

            if (waypoints[i].gameObject.GetComponent<Waypoint>() == null) { waypoints[i].gameObject.AddComponent<Waypoint>(); }

            SetHeight(waypoints[i]);
        }
    }


    /// <summary>
    /// Sets parm waypoint to terrian height + height
    /// </summary>
    /// <param name="waypoint"></param>
    public void SetHeight(Transform waypoint)
    {
        Ray ray = new Ray(waypoint.position, Vector3.down);
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
        {
            waypoint.position = new Vector3(waypoint.position.x, rayHit.point.y + height, waypoint.position.z);
        }
        else
        {
            waypoint.transform.Translate(new Vector3(0, 5, 0));
        }

        oldHeight = height;
    }

    /// <summary>
    /// draws lines connecting the waypoints
    /// </summary>
    void OnDrawGizmos()
    {
        if (waypoints == null) { return; }

        Gizmos.color = pathColor;
        canCheckWaypoints = false;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (i > 0)
            {
                Vector3 pos = waypoints[i].position;
                Vector3 prev = waypoints[i - 1].position;
                Gizmos.DrawLine(prev, pos);
            }
        }
        canCheckWaypoints = true;
    }
}
