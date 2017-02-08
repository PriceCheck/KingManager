using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public class VillageRoadSegment : MonoBehaviour {
    [HideInInspector]
    public VillageNode[] connectedVillages = new VillageNode[2];
    public float Cost = 0;
	// Use this for initialization
	void Start () {
        MessageDispatcher.AddListener("INIT_MapConnector", Init);
	}
	
    public void RecalcuateCost()
    {
        float totalDist = 0;
        Vector3[] positions = new Vector3[GetComponent<LineRenderer>().numPositions];
        GetComponent<LineRenderer>().GetPositions(positions);
        for (int i = 1; i < positions.Length; ++i)
        {
            totalDist += Vector3.Distance(positions[i], positions[i - 1]);
        }
        Cost = totalDist;
    }

    public void SetCost(int NewCost)
    {
        Cost = NewCost;
        MapConnector.instance.UpdateConnectionGraph(this);
    }

    void Init(IMessage mess)
    {

    }

    // Update is called once per frame
    void Update () {
		
	}
}
