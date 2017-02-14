using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
using UnityEngine.UI;

public enum WindowTypes { UnitTraining, HeroInfoScreen, VillageInfoScreen, MessengerInfoScreen};

public struct WindowSpawnEvent
{
    public WindowTypes windowType;
    public Vector2 WindowSpawnLocation;
    public GameObject infoToFill;
}

public class UiSpawner : MonoBehaviour {
    public static UiSpawner inst;
    public static Canvas masterCanvas;
    public GameObject[] UIPrefabs;
    int nextWindowSortingLayer;
    List<GameObject> ActiveWindows = new List<GameObject>(5);
	// Use this for initialization
	void Start () {
        inst = this;
        masterCanvas = GetComponent<Canvas>();
        MessageDispatcher.AddListener("UI_Spawn", HandleUISpawnMessage);
	}

    void Update()
    {
        bool SendRaycastToGameplay = true;
        foreach(GameObject obj in ActiveWindows)
        {
            if(obj.GetComponent<TogglePanel>().inFocus)
            {
                SendRaycastToGameplay = false;
            }
        }
        if(SendRaycastToGameplay)
        {
            GameplayRaycaster.inst.UpdateGameplay();
        }
    }
	
    void HandleUISpawnMessage(IMessage mess)
    {
        switch (((WindowSpawnEvent)mess.Data).windowType)
        {
            case (WindowTypes.UnitTraining):
                print("Unit Training Window");

                break;
            case (WindowTypes.HeroInfoScreen):
                break;
            case (WindowTypes.MessengerInfoScreen):
                break;
            case (WindowTypes.VillageInfoScreen):
                GameObject obj = Instantiate(UIPrefabs[(int)WindowTypes.UnitTraining]);
                obj.transform.parent = masterCanvas.transform;
                obj.transform.localScale = new Vector3(1,1,1);
                obj.GetComponent<RectTransform>().anchoredPosition = ((WindowSpawnEvent)mess.Data).WindowSpawnLocation;
                obj.GetComponent<UnitTrainer>().VillageID = ((VillageNode)mess.Sender).ID;
                break;
            default:
                print("it broke: " + ((WindowSpawnEvent)mess.Data).windowType);
                break;
        }
    }
}
