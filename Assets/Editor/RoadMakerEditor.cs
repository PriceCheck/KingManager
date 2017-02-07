using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoadMaker))]
public class RoadMakerEditor : Editor {
    SerializedProperty VillageConnections;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        RoadMaker script = (RoadMaker)target;
        if (GUILayout.Button("RefreshConnections"))
        {
            script.RecalulateConnections();
        }
        base.OnInspectorGUI();
        if (GUILayout.Button("Build Road"))
        {

            script.makeRoad(); 
        }
        serializedObject.ApplyModifiedProperties();
    }
}
