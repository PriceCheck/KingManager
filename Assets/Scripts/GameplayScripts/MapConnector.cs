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

    void NodeClicked()
    {
        //TODO:
        //If path has been found already, clear nodes clicked
        //When node clicked, add it to a list
        
        //If a node has already been clicked, Calculate the path beteewn the nodes
        //Print the path
        
    }

    void ClearNodesClicked()
    {
        //ToDo:
        //Clear nodes from the list
        
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

    struct Node
    {
        public int previousNode;
        public float Distance;
    }
    //Doing Goddamn Djikstra
    int[] Djikstra(int StartingPoint, int Destination)
    {
        Node[] VertexList = new Node[ConnectionGraph.Length];
        List<int> NodesToVisit = new List<int>();
        for (int i = 0; i < VertexList.Length; ++i)
        {
            Node input;
            input.previousNode = -1;
            input.Distance = float.MaxValue;
            VertexList[i] = input;
        }
        // VertexList[StartingPoint]
        VertexList[StartingPoint].Distance = 0;

        while(NodesToVisit.Count > 0)
        {
            int nextNode = 0;
            int IndexToRemove = 0;
            for(int i = 1; i < NodesToVisit.Count; ++i)
            {
                if(VertexList[nextNode].Distance > VertexList[NodesToVisit[i]].Distance)
                {
                    nextNode = NodesToVisit[i];
                    IndexToRemove = i;
                }
            }
            NodesToVisit.RemoveAt(IndexToRemove);
            for(int i = 0; i < ConnectionGraph[nextNode].Length; ++i)
            {
                if(ConnectionGraph[nextNode][i] != -1 && VertexList[i].Distance > ConnectionGraph[nextNode][i])
                {
                    VertexList[i].Distance = ConnectionGraph[nextNode][i];
                    VertexList[i].previousNode = nextNode;
                    if(i == Destination)
                    {
                        return GetPathFromDestination(VertexList, Destination);
                    }
                }
            }
            
        }
        //Should never reach this point
        return null;
    }

    int[] GetPathFromDestination (Node[] VertexList, int EndPoint)
    {
        List<int> output = new List<int>();
        int currentIndex = EndPoint;
        output.Add(EndPoint);
        while(VertexList[currentIndex].previousNode != -1)
        {
            output.Add(VertexList[currentIndex].previousNode);
            currentIndex = VertexList[currentIndex].previousNode;
        }
        return output.ToArray();
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
