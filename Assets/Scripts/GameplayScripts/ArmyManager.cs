using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.ootii.Messages;

[System.Serializable]
public struct ArmyComp
{
    public UnitType type;
    public int amount;
}

public class ArmyManager : MonoBehaviour {
    public Vector3 FriendlyStartingPos = Vector3.zero;
    public Vector3 EnemyStartingPos = Vector3.zero;
    public Sprite[] UnitSprites;
    public GameObject BaseUnit;
    public GameObject ArmyPrefab;

    void Start()
    {
        MessageDispatcher.AddListener("SAVE_LoadGame", LoadArmies);
        MessageDispatcher.AddListener("SAVE_UnloadGame", UnloadArmies);
    }
    

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            List<Unit> startingUnit = new List<Unit>();
            SpawnNewArmy(FindObjectOfType<VillageNode>(), startingUnit);
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            SaveGame.Save();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            SaveGame.Load();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Game.current.AllArmies[0].AddUnit(UnitType.Archer);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Game.current.AllArmies[0].AddUnit(UnitType.Foot_Solider);
        }

    }
    // Helpers
    public void SpawnNewArmy(VillageNode CurrentNode, List<Unit> StartingUnits)
    {
        Army newArmy = new Army();
        GameObject repsentation = Instantiate(ArmyPrefab, CurrentNode.transform.position, Quaternion.identity);
        newArmy.Repsentation = repsentation;
        newArmy.CurrentLocation = CurrentNode.ID;
        newArmy.UnitsInArmy = StartingUnits;

        Game.current.AllArmies.Add(newArmy);
    }

    public void SpawnExistingArmy(Army army)
    {
        GameObject repsentation = Instantiate(ArmyPrefab, MapConnector.instance.currentVillages[army.CurrentLocation].transform.position, Quaternion.identity);
        army.Repsentation = repsentation;
        Unit[] array = army.UnitsInArmy.ToArray();
        for(int i = 0; i < array.Length; ++i)
        {
            print(array[i].type);
        }
    }

    public void DestoryCurrentArmies()
    {
        foreach(Army arm in Game.current.AllArmies)
        {
            Destroy(arm.Repsentation);
        }
        Game.current.AllArmies = null;
    }

    void UnloadArmies(IMessage mess)
    {
        DestoryCurrentArmies();
    }

    void LoadArmies(IMessage mess)
    {
        foreach (Army arm in Game.current.AllArmies)
        {
            SpawnExistingArmy(arm);
        }
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
}
