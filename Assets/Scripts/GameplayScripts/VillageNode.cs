using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

[System.Serializable]
public struct ConnectionInfo
{
    public int VillageNodeID;
    public int ArrayElementToStartAt;
    public float Cost;
    public VillageRoadSegment Road;
}

[System.Serializable]
public struct UnitTrainingInfo
{
    public UnitType unitType;
    public int UnitCount;
    public float currentTrainingTime;
}

[System.Serializable]
public class VillageNode : MonoBehaviour {
    public Queue<UnitTrainingInfo> queuedUnitsToTrain = new Queue<UnitTrainingInfo>(4);
    public UnitTrainingInfo CurrentTrainingUnit;
    string VillageName = "Test";
    public int ID;
    [HideInInspector]
    public bool IsTraining = false;
    float timer = 0;
    [SerializeField]
    public ConnectionInfo[] Connections;
    [HideInInspector]
    [SerializeField]
    public int currentConnectedVillages = 0;
    public Material[] NodeColors = new Material[3]; // Highlit & non-Highlit
                                                    // Use this for initialization
    void Awake()
    {
    }
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
    public Vector3[] PathToNextNode(int ID)
    {
        for(int i = 0; i < Connections.Length; ++i)
        {
            if(Connections[i].VillageNodeID == ID)
            {
                Vector3[] newArray = new Vector3[Connections[i].Road.GetComponent<LineRenderer>().numPositions]; 
                Connections[i].Road.GetComponent<LineRenderer>().GetPositions(newArray);
                if (GetClosestIndex(transform.position, Connections[i].Road) != 0)
                {
                    Array.Reverse(newArray);
                }
                return newArray;
            }
        }
        //Should never reach here
        return null;
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
        //print("Node: " + ID + " Selected");
        GetComponent<MeshRenderer>().material = NodeColors[1];
    }
    public void OpenThisUI()
    {

        WindowSpawnEvent output;
        output.WindowSpawnLocation = Vector3.zero;
        output.windowType = WindowTypes.VillageInfoScreen;
        output.infoToFill = transform.gameObject;
        MessageDispatcher.SendMessage(this, "UI_Spawn", output, 0);
    }
    public void Clicked()
    {
        IsSelected();
        //MapConnector.instance.NodeClicked(this);
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
            if(bestDistSoFar > Vector3.Distance(Position, positions[i]))
            {
                bestSoFar = i;
                bestDistSoFar = Vector3.Distance(Position, positions[i]);
            }
        }
        return bestSoFar;
    }
    public void TrainNewUnit(UnitTrainingInfo info)
    {
        if(queuedUnitsToTrain.Count < 4)
        {
            queuedUnitsToTrain.Enqueue(info);
        }
    }
    void Update()
    {
        timer += Time.deltaTime;
        if(queuedUnitsToTrain.Count > 0 && !IsTraining)
        {
            IsTraining = true;
            CurrentTrainingUnit = queuedUnitsToTrain.Dequeue();
            timer = 0;
        }
        //ToDo: Change this to be less terrible
        if(timer >= 0 && IsTraining)
        {
            ArmyManager.inst.SpawnUnit(ID,CurrentTrainingUnit.unitType,CurrentTrainingUnit.UnitCount);
            IsTraining = false;
        }

    }
}
