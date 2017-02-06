using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitalizer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        if(Game.current == null)
        {
            Game.current = new Game();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
