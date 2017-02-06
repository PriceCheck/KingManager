using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct CombatResults
{
    public int AlliedHealth;
    public int EnemyHealth;
}


public class BattleController : MonoBehaviour {
    public static BattleController instance;
    void Awake()
    {
        instance = this;
    }
    public static int BattleFieldSize = 8;
    public static void SimulateBattle(Unit[] friendlyArmy, Unit[] enemyArmy)
    {
        //Take Both sides of a conflict
        //a side is An Array of X units
        //int winner = 0;
        List<Unit> FriendlyArmyLocal = new List<Unit>(friendlyArmy);
        List<Unit> EnemyArmyLocal = new List<Unit>(enemyArmy);
        instance.StartCoroutine(instance.Battle(FriendlyArmyLocal, EnemyArmyLocal));
   /*     if (FriendlyArmyLocal.Count <= 0)
        {
            Debug.Log("Enemy Army Won!");
            winner = -1;
        }
        if (EnemyArmyLocal.Count <= 0)
        {
            Debug.Log("Friendly Army won!");
            winner = 1;
        }

        return winner; //Battle was a tie*/
    }


    IEnumerator Battle(List<Unit> FriendlyArmyLocal, List<Unit> EnemyArmyLocal)
    {
        while (FriendlyArmyLocal.Count > 0 && EnemyArmyLocal.Count > 0)
        {
            
            //The battle field has a max number of units that can fight per side, default 8
            int[] FriendUnitsFielded = FieldUnits(FriendlyArmyLocal.ToArray());
            int[] enemyUnitsFielded = FieldUnits(EnemyArmyLocal.ToArray());
            //Each unit has a specific number of units it can fight at one time.
            //Once both sides have all of their units chosen, pair the units off. 
            //TODO: Pair off in a more intellegent way
            for (int i = 0; i < FriendUnitsFielded.Length; ++i)
            {
                yield return new WaitForSeconds(0.5f);
                CombatResults results = Fight(FriendlyArmyLocal[FriendUnitsFielded[i]], EnemyArmyLocal[enemyUnitsFielded[i]]);
            }
            //If One side cannot fill all of their slots, then the extra double up on the 
            // existing units. They are surrounded
            // Roll hit v dodge + any other special attacks that they have.
            //do THIS NEXT
            // Clean up any dead troops
            for (int i = 0; i < FriendlyArmyLocal.Count; ++i)
            {
                if (FriendlyArmyLocal[i].thisUnitStat.Hit_Points <= 0)
                {
                    FriendlyArmyLocal.RemoveAt(i);
                    i = 0;
                }
            }
            for (int i = 0; i < EnemyArmyLocal.Count; ++i)
            {
                if (EnemyArmyLocal[i].thisUnitStat.Hit_Points <= 0)
                {
                    EnemyArmyLocal.RemoveAt(i);
                    i = 0;
                }
            }

            //Repeat until one side is dead.
        }
    }

    static CombatResults Fight(Unit FriendlyUnit, Unit EnemyUnit)
    {
        CombatResults results = new CombatResults();
        Unit[] CombatOrder = new Unit[2];
        if(FriendlyUnit.thisUnitStat.Speed == EnemyUnit.thisUnitStat.Speed)
        {
            //Pick the unit randomly
            int unitGoesFirst = Random.Range(0, 1);
            if(unitGoesFirst == 0)
            {
                CombatOrder[0] = FriendlyUnit;
                CombatOrder[1] = EnemyUnit;
            }
            else
            {
                CombatOrder[1] = FriendlyUnit;
                CombatOrder[0] = EnemyUnit;
            }
        }
        else if(FriendlyUnit.thisUnitStat.Speed > EnemyUnit.thisUnitStat.Speed)
        {
            CombatOrder[0] = FriendlyUnit;
            CombatOrder[1] = EnemyUnit;
        }
        else
        {
            CombatOrder[1] = FriendlyUnit;
            CombatOrder[0] = EnemyUnit;
        }

        CombatOrder[0].Attack(CombatOrder[1]);
        CombatOrder[1].Attack(CombatOrder[0]);
        results.AlliedHealth = FriendlyUnit.thisUnitStat.Hit_Points;
        results.EnemyHealth = EnemyUnit.thisUnitStat.Hit_Points;
        return results;
    }



    static int[] FieldUnits(Unit[] Array)
    {
        List<int> UnitsInCombat = new List<int>();
        int CurrentFieldedUnits = 0;
        for (int i = 0; i < Array.Length; ++i)
        {
            if (Array[i].thisUnitStat.Size + CurrentFieldedUnits <= BattleFieldSize)
            {
                UnitsInCombat.Add(i);
                CurrentFieldedUnits += Array[i].thisUnitStat.Size;
                if (CurrentFieldedUnits == BattleFieldSize)
                {
                    break;
                }
            }
        }
        UnitsInCombat.TrimExcess();
        return UnitsInCombat.ToArray();
    }
}
