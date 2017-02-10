using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public class TogglePanel : MonoBehaviour
{
    public bool inFocus = false;

    public void OnMouseEnter()
    {
        inFocus = true;
    }

    public void OnMouseExit()
    {
        inFocus = false;
    }

    public void TurnOff()
    {
        Destroy(this.gameObject);
    }

    public void TurnOn()
    {

    }
}
