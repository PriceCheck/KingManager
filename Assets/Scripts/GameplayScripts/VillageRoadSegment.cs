using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public class VillageRoadSegment : MonoBehaviour {
    [HideInInspector]
    public VillageNode[] connectedVillages = new VillageNode[2];
    public int Cost = 0;
	// Use this for initialization
	void Start () {
        MessageDispatcher.AddListener("INIT_MapConnector", Init);
	}
	
    

    void Init(IMessage mess)
    {

    }

    // Update is called once per frame
    void Update () {
		
	}
}
