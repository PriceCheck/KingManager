using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ConnectionInfo
{
    public int VillageNodeID;
    public int ArrayElementToStartAt;
    public int Cost;
    public VillageRoadSegment Road;
}

[System.Serializable]
public class VillageNode : MonoBehaviour {
    public int ID;
    [SerializeField]
    public ConnectionInfo[] Connections;
    [HideInInspector]
    [SerializeField]
    public int currentConnectedVillages = 0;
    public Material[] NodeColors = new Material[3]; // Highlit & non-Highlit
	// Use this for initialization
    public void AddRoad(ConnectionInfo info)
    {
        if (Connections.Length == currentConnectedVillages)
        {
            ConnectionInfo[] newArray = new ConnectionInfo[Connections.Length * 2];
            for(int i = 0; i < Connections.Length; ++i)
            {
                newArray[i] = Connections[i];
            }
            Connections = newArray;
        }
        Connections[currentConnectedVillages] = new ConnectionInfo();
        Connections[currentConnectedVillages++] = info;
       // Connections[currentConnectedVillages++].ArrayElementToStartAt = GetClosestIndex(transform.position, info.Road);
      
     //   print(this.gameObject.name + " info: " + Connections[currentConnectedVillages -1].Cost + ", " + Connections[currentConnectedVillages -1].VillageNodeID + ", " + currentConnectedVillages);
    }

    public void isInPath()
    {
        print("Node: " + ID + " Added to path");
        GetComponent<MeshRenderer>().material = NodeColors[2];
    }
    public void isHighlit()
    {

    }



    public void isUnhighlit()
    {
        
    }

    public void isUnselected()
    {
        print("Node: " + ID + " Unselected");
        GetComponent<MeshRenderer>().material = NodeColors[0];
    }

    public void IsSelected()
    {
        print("Node: " + ID + " Selected");
        GetComponent<MeshRenderer>().material = NodeColors[1];
    }

    public void Clicked()
    {

        MapConnector.instance.NodeClicked(this);
        

    }
    /*
    public bool RoadToTravel(int Destination, out int StartingElement, out VillageRoadSegment RoadToTravel)
    {
        foreach(ConnectionInfo road in Connections)
        {
            if(Destination == road.VillageNodeID)
            {
                RoadToTravel = road.Road;
                StartingElement = GetClosestIndex(transform.position, road.Road);
                return true;
            }
        }
        StartingElement = -1;
        RoadToTravel = null;
        return false;
    }

    public int findConnectionCost(int ID)
    {
        foreach(ConnectionInfo info in Connections)
        {
            if(info.VillageNodeID == ID)
            {
                return info.Cost;
            }
        }
        return -1;
    }
    */
    public int GetClosestIndex(Vector3 Position, VillageRoadSegment seg)
    {
        int bestSoFar = -1;
        float bestDistSoFar = float.MaxValue;
        //print(seg.GetComponent<LineRenderer>());
        Vector3[] positions = new Vector3[seg.GetComponent<LineRenderer>().numPositions];
        seg.GetComponent<LineRenderer>().GetPositions(positions);
        for (int i = 0; i < positions.Length; ++i)
        {
            if(bestDistSoFar < Vector3.Distance(Position, positions[i]))
            {
                bestSoFar = i;
                bestDistSoFar = Vector3.Distance(Position, positions[i]);
            }
        }
        return bestSoFar;
    }
}
