using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMaker : MonoBehaviour {
    public VillageNode startingNode;
    public VillageNode endingNode;
    public GameObject RoadPrefab;
    public int Cost = 0;
    public void makeRoad()
    {
       GameObject newRoad = Instantiate(RoadPrefab, Vector3.zero, Quaternion.identity);
       newRoad.GetComponent<VillageRoadSegment>().connectedVillages = new VillageNode[] { startingNode, endingNode };
       newRoad.GetComponent<LineRenderer>().SetPositions(new Vector3[] { startingNode.transform.position, endingNode.transform.position });
        //Update Connection Info
        ConnectionInfo newInfo = new ConnectionInfo();
        newInfo.Cost = Cost;
        newInfo.Road = GetComponent<VillageRoadSegment>();
        //Setting info in starting node
        newInfo.VillageNodeID = endingNode.ID;
        newInfo.ArrayElementToStartAt = -1;//startingNode.GetClosestIndex(startingNode.transform.position,newInfo.Road);
        startingNode.AddRoad(newInfo);
        //Setting info in ending node
        newInfo.VillageNodeID = startingNode.ID;
        newInfo.ArrayElementToStartAt = -1;//endingNode.GetClosestIndex(endingNode.transform.position, newInfo.Road);
        endingNode.AddRoad(newInfo); 
        //Setting name
        newRoad.name = "Road_" + startingNode.name + "->" + endingNode.name;
    }


    public void RecalulateConnections()
    {
        VillageNode[] villages = FindObjectsOfType<VillageNode>();
        VillageRoadSegment[] roads = FindObjectsOfType<VillageRoadSegment>();

        for(int i = 0; i < villages.Length; ++i)
        {
            villages[i].currentConnectedVillages = 0;
        }
        for(int i = 0; i < roads.Length; ++i)
        {
            roads[i].RecalcuateCost();
        }

        for(int i = 0; i < roads.Length; ++i)
        {
            ConnectionInfo newInfo = new ConnectionInfo();
            newInfo.Cost = roads[i].Cost;
            newInfo.Road = roads[i];
            //Setting info in starting node
            newInfo.VillageNodeID = roads[i].connectedVillages[1].ID;
            newInfo.ArrayElementToStartAt = -1;//roads[i].connectedVillages[0].GetClosestIndex(roads[i].connectedVillages[0].transform.position, newInfo.Road);
            roads[i].connectedVillages[0].AddRoad(newInfo);
            //Setting info in ending node
            newInfo.VillageNodeID = roads[i].connectedVillages[0].ID;
            newInfo.ArrayElementToStartAt = -1;//roads[i].connectedVillages[1].GetClosestIndex(roads[i].connectedVillages[1].transform.position, newInfo.Road);
            roads[i].connectedVillages[1].AddRoad(newInfo);
        }
    }
}
