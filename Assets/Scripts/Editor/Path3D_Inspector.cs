#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// custom editor inspector for 'Path3D'
/// will only compile in Editor
/// </summary>
[CustomEditor(typeof(Path3D))]
public class Path3D_Inspector : Editor
{
    #region Variables

    bool placeNewWaypoints;

    Path3D path3D;
    #endregion

    public override void OnInspectorGUI() 
    {
		//get the selected object the inspector is revealing
        path3D = target as Path3D;

        if (placeNewWaypoints) { GUI.backgroundColor = Color.green; }
        else { GUI.backgroundColor = Color.white; }

		GUILayout.BeginHorizontal();
		GUILayout.Space(5);
		GUILayout.BeginVertical();
		GUILayout.Space(5);
		
		GUILayout.BeginVertical("Box");

		    GUILayout.Space(5);
                GUILayout.BeginVertical("Box");

                path3D.pathColor = EditorGUILayout.ColorField("Path Color", path3D.pathColor, GUILayout.Width(250));
                    path3D.height = EditorGUILayout.Slider("Height", path3D.height, 3, 100);

                GUILayout.EndVertical();

                if (GUILayout.Button("Place Waypoints", GUILayout.Width(150)))
                {
                    placeNewWaypoints = !placeNewWaypoints;
                }

            GUILayout.EndVertical();

		    GUILayout.Space(5);
		    GUILayout.EndHorizontal();
		    GUILayout.Space(5);

		GUILayout.EndVertical();
		
		//check for changes in values
		if (GUI.changed) 
        {
            // ----------- If height has changed, set height -----------------------
            if (path3D.oldHeight != path3D.height)
            {
                for (int i = 0; i < path3D.waypoints.Length; i++)
                {
                    path3D.SetHeight(path3D.waypoints[i]);
                }
            }
		}
	}

    /// <summary>
    /// Called when a ui event occurs in the editor window
    /// </summary>
    /// <param name="sceneView"></param>
    void OnSceneGUI()
    {

        Event cur = Event.current;

        // --------------------- Click Place waypoints -----------------------------
        if (cur.type == EventType.MouseDown && placeNewWaypoints && cur.button == 0)
        {

            UnityEditor.EditorWindow window = UnityEditor.EditorWindow.GetWindow<UnityEditor.EditorWindow>();
            Vector2 res = EditorUtils.GetMainGameViewSize();
            float xPos = cur.mousePosition.x / res.x;
            float yPos = 1 - (cur.mousePosition.y / res.y);

            Ray ray = Camera.current.ViewportPointToRay(new Vector3(xPos, yPos, 0));

            RaycastHit rayhit;
            if (Physics.Raycast(ray, out rayhit, Mathf.Infinity))
            {
                path3D.AddWaypoint(rayhit.point);
            }
        }

        // --------------------- Disable viewport selecting --------------------------
        if (placeNewWaypoints) { HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive)); }
        else { HandleUtility.Repaint(); }
    }

}
#endif