using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Army {
    public int ID;
    public List<Unit> UnitsInArmy;
    public int CurrentLocation;
    public int MoveSpeed = 8;
    public AnimationCurve MoveSpeedRateChange = AnimationCurve.Linear(1, 0.25f, 10, 3);
    [HideInInspector]
    public int[] Path;
    int PathIndex = 0;
    Vector3[] WorldSpacePoints;
    int PointsIndex = 0;
    public bool isMoving = false;
    float timer = 0;
    float TimeUntilNext = 0;
    Vector3 StartMovePoint;
    Vector3 EndMovePoint;
    Vector3 MoveDelta;
    [System.NonSerialized]
    public GameObject Repsentation;

    public void Update()
    {
        if(isMoving)
        {
            UpdateMovement();
        }
    }

    public void SetPath(int[] input)
    {
        Path = input;
        PathIndex = 0;
        isMoving = true;
    }
    public void NextPathPoint()
    {
        ++PathIndex;
        onEnterVillage(PathIndex);
        PointsIndex = 0;
        if(PathIndex >= Path.Length)
        {
            CurrentLocation = Path[Path.Length - 1];
            Path = null;
            return;
        }
        //Go to next index in the high level path
        //Get the positions from that village node
        WorldSpacePoints = MapConnector.instance.currentVillages[Path[PathIndex -1]].PathToNextNode(Path[PathIndex]);
        //Go the the next World Space point
        NextWorldSpacePoint();
    }
    public void NextWorldSpacePoint()
    {
        ++PointsIndex;
        if(WorldSpacePoints == null || PointsIndex >= WorldSpacePoints.Length)
        {
            WorldSpacePoints = null;
            timer = 0;
            TimeUntilNext = 0;
            return;
        }
        timer = 0;
        TimeUntilNext = Vector3.Distance(WorldSpacePoints[PointsIndex - 1], WorldSpacePoints[PointsIndex]);
        StartMovePoint = WorldSpacePoints[PointsIndex - 1];
        EndMovePoint = WorldSpacePoints[PointsIndex];
        MoveDelta = EndMovePoint - StartMovePoint;
    }
    public void IsSelected()
    {
        Repsentation.GetComponent<ArmyRepresentation>().IsSelected();
    }
    void onEnterVillage(int Index)
    {
        CurrentLocation = Index;
        foreach (Army arm in Game.current.AllArmies)
        {
            if (arm.CurrentLocation == CurrentLocation && !arm.isMoving)
            {
                MergeArmies(arm);
            }
        }
    }
    void UpdateMovement()
    {
        timer += TimeController.SmoothDTthisframe * MoveSpeedRateChange.Evaluate(MoveSpeed);
        if(timer >= TimeUntilNext)
        {
            if(TimeUntilNext != 0)
            {
                timer = TimeUntilNext;
                UpdatePosition();
            }
            NextWorldSpacePoint();
        }
        else
        {
            UpdatePosition();
        }
        if (WorldSpacePoints == null)
        {
            NextPathPoint();
        }
        if(Path == null)
        {
            isMoving = false;
        }
    }
    void UpdatePosition()
    {
        Repsentation.transform.position = StartMovePoint + (MoveDelta * (timer / TimeUntilNext));
    }
    public void MoveToVillage(int ID)
    {
        Repsentation.GetComponent<ArmyRepresentation>().Prnt("Movement Started");
        SetPath(MapConnector.instance.Djikstra(CurrentLocation, ID));

    }
   public void AddUnit(UnitType type)
    {
        Unit newUnit = new Unit(type);
        UnitsInArmy.Add(newUnit);
    }
    public void MergeArmies(Army rhs)
    {
        Repsentation.GetComponent<ArmyRepresentation>().Prnt("Attempt Merge");
        UnitsInArmy.AddRange(rhs.UnitsInArmy);
        Repsentation.GetComponent<ArmyRepresentation>().Prnt("Units Added");
        ArmyManager.inst.DestroyArmy(rhs.ID);
        Repsentation.GetComponent<ArmyRepresentation>().Prnt("Destroyed Army");
    }

}
