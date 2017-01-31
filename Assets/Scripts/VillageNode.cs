using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageNode : MonoBehaviour {
    [HideInInspector]
    public int ID;
    static int NextID = 0;
    public VillageNode[] ConnectedNodes;
    public Material[] NodeColors = new Material[2]; // Highlit & non-Highlit
    public int[] NodeCosts;
	// Use this for initialization
	void Start () {
        ID = NextID++;

	}
	
    void Reset()
    {
        NextID = 0;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
