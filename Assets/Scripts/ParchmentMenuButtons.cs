using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public class ParchmentMenuButtons : MonoBehaviour {
    bool ishovered = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(ishovered && Input.GetMouseButtonDown(0))
        {
            MessageDispatcher.SendMessage(this, "LEVELS_StartLevelChange", 0, 0);
            ishovered = false;
        }
	}

    public void OnMouseExit()
    {
        ishovered = false;
    }

    public void OnMouseEnter()
    {
        ishovered = true;
    }
}
