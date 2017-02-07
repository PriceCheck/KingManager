using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public class MapConnector : MonoBehaviour
{
    public static MapConnector instance;
    public int[][] ConnectionGraph = null;
  //  ConnectionInfo[] allConnections;
    [HideInInspector]
    public VillageNode[] currentVillages;
    VillageRoadSegment[] currentRoads;
    int numVillagesSelected = 0;
    VillageNode[] VillagesSelected = new VillageNode[2];
    // Use this for initialization
    void Start()
    {
        instance = this;
        MessageDispatcher.AddListener("INPUT_ObjectClicked", CheckClick);
        StartCoroutine(WaitForFixed());
    }

    public void NodeClicked(VillageNode clickedNode)
    {
        print("NodeClicked!");
        //If path has been found already, clear nodes clicked
        if (numVillagesSelected == 2)
        { 
            ClearNodesClicked();
        }
        //When node clicked, add it to a list
        VillagesSelected[numVillagesSelected] = clickedNode;
        ++numVillagesSelected;
        //If a node has already been clicked, Calculate the path beteewn the nodes
        if(numVillagesSelected == 2)
        {
            print("Printing Path");
            PrintPath(Djikstra(VillagesSelected[0].ID, VillagesSelected[1].ID));
        }
        //Print the path
        
    }

    void CheckClick(IMessage mess)
    {
        GameObject lastObjHit = (GameObject)mess.Data;
        if(numVillagesSelected > 0 && !lastObjHit.GetComponent<VillageNode>())
        {
            ClearNodesClicked();
        }
    }


    void ClearNodesClicked()
    {
       // print("NodesCleared");
        //Clear nodes from the list
        if (VillagesSelected[0])
        {
            VillagesSelected[0].isUnhighlit();
            //Array Slot 1 will only be filled if 0 is
            if (VillagesSelected[1])
            {
                VillagesSelected[1].isUnhighlit();
            }
        }
        VillagesSelected[0] = null;
        VillagesSelected[1] = null;
        numVillagesSelected = 0;
        
    }

    void init()
    {
        currentVillages = FindObjectsOfType<VillageNode>();
        currentRoads = FindObjectsOfType<VillageRoadSegment>();
        PopulateConnectionGraph();
        
        PrintConnectionGraph();
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
    
    void PrintPath(int[] VertexPath)
    {
        string output = "(" + VertexPath[0];
        for(int i = 1; i < VertexPath.Length; ++i)
        {
           output += "->" + VertexPath[i];
        }
        output += ")";
        print(output);
       
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
            print(currentVillages[i].name + ": " + currentVillages[i].currentConnectedVillages);
            for (int j = 0; j < currentVillages[i].currentConnectedVillages; ++j)
            {
                print("Cost: " + currentVillages[j].Connections[j].Cost);
                ConnectionGraph[currentVillages[i].ID][currentVillages[i].Connections[j].VillageNodeID]
                    = currentVillages[i].Connections[j].Cost;
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
