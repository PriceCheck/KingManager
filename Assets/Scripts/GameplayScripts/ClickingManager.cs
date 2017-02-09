using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
[System.Serializable]
public enum ObjectType { Army, Village, Monster, Hero, Messenger, Deselect };
[System.Serializable]
public enum ClickEventType { Normal, Wild };
//0 is left click, 1 is right click
[System.Serializable]
public struct ClickEvent
{
    public ClickEvent(ObjectType type_, int mouseButton_, bool WillBeTrue_ = false)
    {
        Object = type_;
        ClickType = mouseButton_;
        GameObject = null;
        WillBeTrue = WillBeTrue_;
    }
    public bool equalTo(ClickEvent rhs)
    {
        return WillBeTrue || (Object == rhs.Object && ClickType == rhs.ClickType);
    }
    public ObjectType Object;
    public int ClickType;
    public GameObject GameObject;
    bool WillBeTrue;
}
[System.Serializable]
public delegate void ClickPatternMethod(ClickEvent[] events);
[System.Serializable]
public class ClickingManager : ScriptableObject {
    public static ClickingManager instance;
    public ClickEvent[] storedClicks = new ClickEvent[2] {
        new ClickEvent(ObjectType.Deselect, -1),
        new ClickEvent(ObjectType.Deselect, -1)
        };
    public ClickPatternMethod[] KnownMethods = new ClickPatternMethod[5]
        {   /* Double click Patterns*/
            new ClickPatternMethod(ArmyMoveEvent),
            /* Deselect Patterns*/
            new ClickPatternMethod(DeselectArmy),
            new ClickPatternMethod(DeselectVillage),
            /* Single click Patterns*/
          new ClickPatternMethod(SelectArmy),
          new ClickPatternMethod(SelectVillage)};

    //Patterns of clicking for stuff
    ClickEvent[][] DoubleClickPatterns = new ClickEvent[1][] {
        new ClickEvent[2] { new ClickEvent(ObjectType.Village, 1),  new ClickEvent(ObjectType.Army, 0) },
    };

    ClickEvent[][] DeselectPatterns = new ClickEvent[2][]
        {
        new ClickEvent[2] { new ClickEvent(ObjectType.Deselect, 0, true), new ClickEvent(ObjectType.Army, 0)},
        new ClickEvent[2] { new ClickEvent(ObjectType.Deselect, 0, true), new ClickEvent(ObjectType.Village, 0)}
        };

    ClickEvent[][] SingleClickPatterns = new ClickEvent[2][] {
            new ClickEvent[1] {new ClickEvent(ObjectType.Army, 0) },
            new ClickEvent[1] {new ClickEvent(ObjectType.Village, 0)}
        };


    void Awake()
    {
        instance = this;
        MessageDispatcher.AddListener("INPUT_ObjectClicked", ObjectLeftClicked);
        MessageDispatcher.AddListener("INPUT_ObjectRightClicked", ObjectRightClicked);
    }

    void HandleClick(GameObject ClickedObj, int ClickType)
    {
        ClickEvent newEvent = new ClickEvent();
        newEvent.ClickType = ClickType;
        switch (ClickedObj.tag)
        {
            case ("Army"):
                {
                    newEvent.Object = ObjectType.Army;
                    newEvent.GameObject = ClickedObj;
                    break;
                }

            case ("Monster"):
                {
                    newEvent.Object = ObjectType.Monster;
                    newEvent.GameObject = ClickedObj;
                    break;
                }
            case ("Messenger"):
                {
                    newEvent.Object = ObjectType.Messenger;
                    newEvent.GameObject = ClickedObj;
                    break;
                }
            case ("Hero"):
                {
                    newEvent.Object = ObjectType.Hero;
                    newEvent.GameObject = ClickedObj;
                    break;
                }
            case ("Village"):
                {
                    newEvent.Object = ObjectType.Village;
                    newEvent.GameObject = ClickedObj;
                    break;
                }
            default:
                  newEvent.Object = ObjectType.Deselect;
                break;
        }

 
        storedClicks[1] = storedClicks[0];
        storedClicks[0] = newEvent;
        //DEBUG
        PrintClickLog();

        if(!CheckForExistingPatterns(DoubleClickPatterns, 0))
        {
            CheckForExistingPatterns(DeselectPatterns, DoubleClickPatterns.Length);
            CheckForExistingPatterns(SingleClickPatterns, DoubleClickPatterns.Length + DeselectPatterns.Length);
        }
    }

    void PrintClickLog()
    {
        if (storedClicks[0].GameObject != null)
            MonoBehaviour.print("Click 0: " + storedClicks[0].GameObject.name + " as " + storedClicks[0].Object + " with button " + storedClicks[0].ClickType);
        else
            MonoBehaviour.print("Click 0: is Null");
        if (storedClicks[1].GameObject != null)
            MonoBehaviour.print("Click 1: " + storedClicks[1].GameObject.name + " as " + storedClicks[1].Object + " with button " + storedClicks[1].ClickType);
        else
            MonoBehaviour.print("Click 1: is Null");
    }

    bool CheckForExistingPatterns(ClickEvent[][] Patterns, int offset)
    {
        int MatchingPattern = -1;
        for (int i = 0; i < Patterns.Length; ++i)
        {
            bool isMatching = true;
            for (int j = 0; j < Patterns[i].Length; ++j)
            {
                if (!(Patterns[i][j].equalTo(storedClicks[j])))
                {
                    isMatching = false;
                }
            }
            if (isMatching)
            {
                MatchingPattern = i;

                break;
            }
        }
        if(MatchingPattern != -1)
        {
            KnownMethods[MatchingPattern + offset](storedClicks);
            return true;
        }
        return false;
    }

    void ObjectLeftClicked(IMessage message)
    {
        HandleClick((GameObject)message.Data, 0);
    }

    void ObjectRightClicked(IMessage message)
    {
        HandleClick((GameObject)message.Data, 1);
    }

    /*Delegate Functions*/
    static void ArmyMoveEvent(ClickEvent[] Objects)
    {
        //Tell the army to move
        (Objects[1].GameObject.GetComponent<ArmyRepresentation>()).MoveToVillage(Objects[0].GameObject.GetComponent<VillageNode>().ID);
        (Objects[1].GameObject.GetComponent<ArmyRepresentation>()).isUnselected();
    }
    static void SelectArmy(ClickEvent[] Objects)
    {
        (Objects[0].GameObject.GetComponent<ArmyRepresentation>()).IsSelected();
    }
    static void SelectVillage(ClickEvent[] Objects)
    {
        (Objects[0].GameObject.GetComponent<VillageNode>()).IsSelected();
    }
    static void DeselectArmy(ClickEvent[] Objects)
    {
        (Objects[1].GameObject.GetComponent<ArmyRepresentation>()).isUnselected();
    }
    static void DeselectVillage(ClickEvent[] Objects)
    {
        (Objects[1].GameObject.GetComponent<VillageNode>()).isUnselected();
    }
}
