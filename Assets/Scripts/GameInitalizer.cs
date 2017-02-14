using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitalizer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (Game.current == null)
        {
            new Game();
        }
    }

    void Awake()
    {

    }
	
	// Update is called once per frame
	void Update () {
		if(Game.current.Loaded == false)
        {
            Game.current.uploadCurrentGameState(FindObjectsOfType<ArmyRepresentation>(), FindObjectsOfType<VillageNode>());
        }
	}
}
