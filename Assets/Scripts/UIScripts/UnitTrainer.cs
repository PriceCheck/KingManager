using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTrainer : MonoBehaviour {

    //Each Unit Trainer Corrisponds to a particular menu & village
    public bool Active = false;
    public int VillageID = -1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Alpha1) && VillageID != -1)
        {
            UnitTrainingInfo info;
            info.unitType = UnitType.Foot_Solider;
            info.UnitCount = 1;
            info.currentTrainingTime = 0;
            print(Game.current.AllNodes.ToString());
            Game.current.AllNodes[VillageID].TrainNewUnit(info);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && VillageID != -1)
         {
            UnitTrainingInfo info;
            info.unitType = UnitType.Archer;
            info.UnitCount = 1;
            info.currentTrainingTime = 0;
            Game.current.AllNodes[VillageID].TrainNewUnit(info);
        }
	}
}
