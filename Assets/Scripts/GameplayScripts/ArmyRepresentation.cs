using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyRepresentation : MonoBehaviour {
    [HideInInspector]
    public Army myArmy;
    public Material[] PieceColors = new Material[3];

    void Update()
    {
        myArmy.Update();
    }

    public void MoveToVillage(int ID)
    { 
        myArmy.MoveToVillage(ID);
    }

    public void toConsole(string output)
    {
        print(output);
    }

    public void isUnhighlit()
    {

    }

    public void isUnselected()
    {
        GetComponent<MeshRenderer>().material = PieceColors[0];
    }

    public void IsSelected()
    {
       // print("IsSelected");
        GetComponent<MeshRenderer>().material = PieceColors[1];
    }

    public void Prnt(string output)

    {
        print(output);
    }

    public void Clicked()
    {

       // MapConnector.instance.NodeClicked(this);


    }
}
