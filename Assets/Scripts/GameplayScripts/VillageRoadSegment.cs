using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageRoadSegment : MonoBehaviour {
    public VillageNode[] connectedVillages = new VillageNode[2];
	// Use this for initialization
	void Start () {
		
	}
	
    void Awake()
    {
        PathInfo info;
        info.Road = this;
        info.ArrayElementToStartAt = -1;
        //Set up connection from 1 --> 0
        info.VillageNodeID = connectedVillages[0].ID;
        connectedVillages[1].AddRoad(info);
        //Set up connection from 0 --> 1
        info.VillageNodeID = connectedVillages[1].ID;
        connectedVillages[0].AddRoad(info);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
