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
            VillageNode[] allVillages = FindObjectsOfType<VillageNode>();
            for (int i = 0; i < allVillages.Length; ++i)
            {
                SerializedObject obj = new UnityEditor.SerializedObject(allVillages[i].GetComponent<VillageNode>());
                script.RecalulateConnections(obj);
            }
        }
        base.OnInspectorGUI();
        if (GUILayout.Button("Build Road"))
        {

            script.makeRoad(); 
        }
        serializedObject.ApplyModifiedProperties();
    }
}
