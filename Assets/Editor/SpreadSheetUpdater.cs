using UnityEngine;
using UnityEditor;

public class SpreadSheetUpdate {
    [MenuItem("Tools/Re-import Generated Scripts")]
    private static void NewMenuOption()
    {
        UnitStatsImporter.UpdateUnitStats();
    }


}
