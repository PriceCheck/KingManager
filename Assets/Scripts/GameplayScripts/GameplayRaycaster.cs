using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public class GameplayRaycaster : MonoBehaviour {
    public static GameplayRaycaster inst;
    Ray ray;
    RaycastHit hit;
    GameObject LastObjectHit;


    void Start()
    {
        ClickingManager.CreateInstance<ClickingManager>();
        inst = this;
    }

    public void UpdateGameplay()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (LastObjectHit != hit.collider.gameObject)
            {
                HighlightingLogic();

                LastObjectHit = hit.collider.gameObject;

            }

        }
        if (Input.GetMouseButtonDown(0))
        {
            if (LastObjectHit != null)
            {
                ClickedLogic();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (LastObjectHit != null)
            {
                RightClickedLogic();
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
            
         //   if (LastObjectHit.GetComponent<VillageNode>())
             //   LastObjectHit.GetComponent<VillageNode>().isUnhighlit();
        }
        // Template
        //if(hit.collider.gameObject.GetComponent</*ComponentNameHere*/>())
        //    hit.collider.gameObject.GetComponent<MainMenuButton>().isHighlit();
       // if(hit.collider.gameObject.GetComponent<VillageNode>())
           // hit.collider.gameObject.GetComponent<VillageNode>().isHighlit();
    }

    void ClickedLogic()
    {
        // Template
        //if(hit.collider.gameObject.GetComponent</*ComponentNameHere*/>())
        //LastObjectHit.GetComponent<>().Clicked();
      //  if (hit.collider.gameObject.GetComponent<VillageNode>())
        { 
          //LastObjectHit.GetComponent<VillageNode>().Clicked();
        }
        MessageDispatcher.SendMessage(this, "INPUT_ObjectClicked", LastObjectHit, 0);
    }

    void RightClickedLogic()
    {
        if (hit.collider.gameObject.GetComponent<VillageNode>())
        {
           // LastObjectHit.GetComponent<VillageNode>().RightClicked();
        }
        MessageDispatcher.SendMessage(this, "INPUT_ObjectRightClicked", LastObjectHit, 0);
    }

    public bool LastObjectHasType<T>()
    {
        return LastObjectHit && LastObjectHit.GetComponent<T>() != null;
    }

}
