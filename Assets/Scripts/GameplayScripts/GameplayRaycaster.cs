﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public class GameplayRaycaster : MonoBehaviour {
    Ray ray;
    RaycastHit hit;
    GameObject LastObjectHit;
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (LastObjectHit != hit.collider.gameObject)
            {
                HighlightingLogic();
            }

        }
        if (Input.GetMouseButtonDown(0))
        {
            if (LastObjectHit != null)
            {
                ClickedLogic();
            }

        }
    }
    void HighlightingLogic()
    {
        //
        if (LastObjectHit != null)
        {
            // Template
            // if (LastObjectHit.GetComponent</*ComponentNameHere*/>())
            // LastObjectHit.GetComponent<>().isUnhighlit();
            
            if (LastObjectHit.GetComponent<VillageNode>())
                LastObjectHit.GetComponent<VillageNode>().isUnhighlit();
        }
        // Template
        //if(hit.collider.gameObject.GetComponent</*ComponentNameHere*/>())
        //    hit.collider.gameObject.GetComponent<MainMenuButton>().isHighlit();
        if(hit.collider.gameObject.GetComponent<VillageNode>())
            hit.collider.gameObject.GetComponent<VillageNode>().isHighlit();

        LastObjectHit = hit.collider.gameObject;
    }

    void ClickedLogic()
    {
        //take Last object Hit
        //LastObjectHit;
        // Template
        //if(hit.collider.gameObject.GetComponent</*ComponentNameHere*/>())
        //LastObjectHit.GetComponent<>().Clicked();
        if (hit.collider.gameObject.GetComponent<VillageNode>())
        { 
          LastObjectHit.GetComponent<VillageNode>().Clicked();
        }
        MessageDispatcher.SendMessage(this, "INPUT_ObjectClicked", LastObjectHit, 0);
    }

    public bool LastObjectHasType<T>()
    {
        return LastObjectHit && LastObjectHit.GetComponent<T>() != null;
    }

}