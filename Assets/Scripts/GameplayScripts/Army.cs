using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Army {
    public List<Unit> UnitsInArmy;
    public int CurrentLocation;
    [System.NonSerialized]
    public GameObject Repsentation;

    void Update()
    {

    }

    void MoveToVillage()
    {

    }

   public void AddUnit(UnitType type)
    {
        Unit newUnit = new Unit(type);
        UnitsInArmy.Add(newUnit);
    }
}
