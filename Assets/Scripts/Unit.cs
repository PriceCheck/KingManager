using UnityEngine;
using System.Collections;

public class Unit {
    public UnitType type;
    public StatsReference.UnitStats thisUnitStat;
    public UnitVisualizer myVisualizer;
    // Use this for initialization
    public Unit(UnitType thisType)
    {
        type = thisType;
        thisUnitStat = StatsReference.UnitStatsArray[(int)thisType];
    }

    public void TakeDamage(int Damage)
    {
        thisUnitStat.Hit_Points -= Damage;
        if(thisUnitStat.Hit_Points > 0)
        {
            myVisualizer.TakeDamage(Damage);
        }
        else
        {
            myVisualizer.Dead();
        }
       
    }

    public void Attack(Unit Target)
    {
        Debug.Log(thisUnitStat.Name + " Attacks " + Target.thisUnitStat.Name);
        if (thisUnitStat.Hit_Points > 0)
        {
            int ToHit = Random.Range(0, 100);
            if (ToHit < thisUnitStat.Accuracy)
            {
                Target.TakeDamage(thisUnitStat.Damage);
                myVisualizer.Attack(thisUnitStat.Damage);
            }
            else
            {
                myVisualizer.Miss();
            }
        }
    }
}
