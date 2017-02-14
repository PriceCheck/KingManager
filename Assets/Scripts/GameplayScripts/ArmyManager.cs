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
    public static ArmyManager inst = null;
    static int nextID = 0;
    public int ID;
    public Vector3 FriendlyStartingPos = Vector3.zero;
    public Vector3 EnemyStartingPos = Vector3.zero;
    public Sprite[] UnitSprites;
    public GameObject BaseUnit;
    public GameObject ArmyPrefab;
    //each village has an army
    //Any units that spawn in a 

    public void SpawnUnit(int villageID, UnitType type, int count)
    {
        int foundVillage = -1;
        for(int i = 0; i < Game.current.AllArmies.Count; ++i)
        {
            if(Game.current.AllArmies[i].CurrentLocation == villageID && !Game.current.AllArmies[i].isMoving)
            {
                foundVillage = i;
                break;
            }
        }

        if(foundVillage == -1)
        {
            List<Unit> newUnits = new List<Unit>(5);
            for (int i = 0; i < count; ++i)
            {
                newUnits.Add(new Unit(type));
            }
            SpawnNewArmy(Game.current.AllNodes[villageID], newUnits);
        }
        else
        {
            for (int i = 0; i < count; ++i)
            {
                Game.current.AllArmies[i].AddUnit(type);
            }
        }
    }

    void Start()
    {
        inst = this;
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
    }
    // Helpers
    public void SpawnNewArmy(VillageNode CurrentNode, List<Unit> StartingUnits)
    {
        Army newArmy = new Army();
        GameObject repsentation = Instantiate(ArmyPrefab, CurrentNode.transform.position, Quaternion.identity);
        newArmy.Repsentation = repsentation;
        repsentation.GetComponent<ArmyRepresentation>().myArmy = newArmy;
        newArmy.CurrentLocation = CurrentNode.ID;
        newArmy.UnitsInArmy = StartingUnits;
        newArmy.ID = Game.current.AllArmies.Count;
        Game.current.AllArmies.Add(newArmy);
    }

    public void SpawnExistingArmy(Army army)
    {
        GameObject repsentation = Instantiate(ArmyPrefab, MapConnector.instance.currentVillages[army.CurrentLocation].transform.position, Quaternion.identity);
        army.Repsentation = repsentation;
        Unit[] array = army.UnitsInArmy.ToArray();
    }

    public void DestoryCurrentArmies()
    {
        foreach(Army arm in Game.current.AllArmies)
        {
            Destroy(arm.Repsentation);
        }
        Game.current.AllArmies = null;
    }

    public void DestroyArmy(int ArmyID)
    {
        Destroy (Game.current.AllArmies[ArmyID].Repsentation);
        Game.current.AllArmies.RemoveAt(ArmyID);
        int i = 0;
        foreach(Army arm in Game.current.AllArmies.ToArray())
        {
            arm.ID = i++;
        }
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
