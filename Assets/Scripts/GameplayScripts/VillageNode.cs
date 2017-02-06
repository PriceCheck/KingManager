using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PathInfo
{
    public int VillageNodeID;
    public int ArrayElementToStartAt;
    public VillageRoadSegment Road;
}

public class VillageNode : MonoBehaviour {
    [HideInInspector]
    public static List<VillageNode> AllNodes = new List<VillageNode>();
    [HideInInspector]
    public int ID;
    static int NextID = 0;
    public VillageNode[] ConnectedNodes;
    List<PathInfo> Roads;
    public Material[] NodeColors = new Material[2]; // Highlit & non-Highlit
    public int[] NodeCosts;
	// Use this for initialization
	void Start () {
        AllNodes.Add(this);
        ID = NextID++;
	}
	
    public void AddRoad(PathInfo info)
    {
        Roads.Add(info);
    }

    public bool RoadToTravel(int Destination, out int StartingElement, out VillageRoadSegment RoadToTravel)
    {
        foreach(PathInfo road in Roads)
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

    int GetClosestIndex(Vector3 Position, VillageRoadSegment seg)
    {
        int bestSoFar = -1;
        float bestDistSoFar = float.MaxValue;
        Vector3[] positions = new Vector3[seg.GetComponent<LineRenderer>().numPositions];
        seg.GetComponent<LineRenderer>().GetPositions(positions);
        for (int i = 0; i < positions.Length; ++i)
        {
            if(bestDistSoFar < Vector3.Distance(Position, positions[i]))
            {
                bestSoFar = i;
                bestDistSoFar = Vector3.Distance(Position, positions[i])
            }
        }
        return bestSoFar;
    }

    void Reset()
    {
        NextID = 0;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
