using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public enum WindowTypes { UnitTraining, HeroInfoScreen, VillageInfoScreen, MessengerInfoScreen};

public struct WindowSpawnEvent
{
    public WindowTypes windowType;
    public Vector3 UnitSpawnLocation;
    GameObject infoToFill;
}

public class UiSpawner : MonoBehaviour {
    public static UiSpawner inst;
    public GameObject[] UIPrefabs;
    int nextWindowSortingLayer;
    List<GameObject> ActiveWindows = new List<GameObject>(5);
	// Use this for initialization
	void Start () {
        inst = this;
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
        switch((WindowTypes)mess.Data)
        {
            case (WindowTypes.UnitTraining):
                break;
            case (WindowTypes.HeroInfoScreen):
                break;
            case (WindowTypes.MessengerInfoScreen):
                break;
            case (WindowTypes.VillageInfoScreen):
                break;
            default:
                break;
        }
    }
}
