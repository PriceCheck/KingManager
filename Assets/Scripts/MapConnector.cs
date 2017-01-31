using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapConnector : MonoBehaviour
{
    public static MapConnector instance;
    public int[][] ConnectionGraph = null;
    VillageNode[] currentVillages;
    VillageRoadSegment[] currentRoads;
    // Use this for initialization
    void Start()
    {
        instance = this;
        StartCoroutine(WaitForFixed());
    }

    void Awake()
    {
        
    }

    void init()
    {
        currentVillages = FindObjectsOfType<VillageNode>();
        currentRoads = FindObjectsOfType<VillageRoadSegment>();
        PopulateConnectionGraph();
        //PrintConnectionGraph();
    }

    IEnumerator WaitForFixed()
    {
        yield return new WaitForFixedUpdate();
        init();

    }

    void PopulateConnectionGraph()
    {
        ConnectionGraph = new int[currentVillages.Length][];
        for(int i = 0; i < ConnectionGraph.Length; ++i)
        {
            ConnectionGraph[i] = new int[currentVillages.Length];
            for(int j = 0; j < ConnectionGraph[i].Length; ++j)
            {
                ConnectionGraph[i][j] = -1;
            }
        }

        for (int i = 0; i < currentVillages.Length; ++i)
        {
            for(int j = 0; j < currentVillages[i].ConnectedNodes.Length; ++j)
            {
                ConnectionGraph[currentVillages[i].ID][currentVillages[i].ConnectedNodes[j].ID]
                    = currentVillages[i].NodeCosts[j];
            }
        }
    }

    void PrintConnectionGraph()
    {
        string output = "{";
        for(int i = 0; i < ConnectionGraph.Length; ++i)
        {
            output = output + "[";
            for (int j = 0; j < ConnectionGraph[i].Length; ++j)
            {
                output = output + ConnectionGraph[i][j] + ", ";
            }
            output = output + "]\n";
        }
        output = output + "}";
        print(output);
    }

    public void OnDestroy()
    {
        instance = null;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
