using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public struct ArmyComp
{
    public UnitType type;
    public int amount;
}

public class ArmyManager : MonoBehaviour {
    public ArmyComp[] FriendlyArmy;
    public ArmyComp[] EnemyArmy;
    public Vector3 FriendlyStartingPos = Vector3.zero;
    public Vector3 EnemyStartingPos = Vector3.zero;
    public Sprite[] UnitSprites;
    public GameObject BaseUnit;
    bool DoOnce = true;
	void Update()
    {
        if(DoOnce && Input.GetKeyDown(KeyCode.A))
        {
            RunBattle();
            DoOnce = false;
        }

    }
    // Use this for initialization
	void RunBattle()
    {
        BattleController.SimulateBattle(
           CreateUnitsFromArmyComp(FriendlyArmy, FriendlyStartingPos, -1),
           CreateUnitsFromArmyComp(EnemyArmy, EnemyStartingPos, 1));
    }

    Unit[] CreateUnitsFromArmyComp(ArmyComp[] army, Vector3 StartingPos, int reverse)
    {
        List<Unit> unitArray = new List<Unit>();
        for (int i = 0; i < army.Length; ++i)
        {
            for (int j = 0; j < army[i].amount; ++j)
            {
                Unit newUnit = new Unit(army[i].type);
                GameObject obj = Instantiate(BaseUnit, StartingPos + (new Vector3(2 * j * reverse, 1 * i, 0)), Quaternion.identity);
                newUnit.myVisualizer = obj.GetComponent<UnitVisualizer>();
                obj.SetActive(true);
                unitArray.Add(newUnit);
            }
        } 
        return unitArray.ToArray();
    }
    [MenuItem("Tools/Run Test Battle")]
    private void NewMenuOption()
    {
        RunBattle();
    }
}
