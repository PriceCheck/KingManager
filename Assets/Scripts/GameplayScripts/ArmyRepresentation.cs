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
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            myArmy.MoveToVillage(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            myArmy.MoveToVillage(2);
        }
    }

    public void MoveToVillage(int ID)
    {
        print("MoveToVillage");
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

    public void Clicked()
    {

       // MapConnector.instance.NodeClicked(this);


    }
}
