using UnityEngine;
using System.Collections;
using com.ootii.Messages;

public class CameraPanner : MonoBehaviour {
    public GameObject[] TargetLocations;
    public AnimationCurve MovementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float MovementTime = 0.7f;
    public Stack LocationsToMoveTo;
    IEnumerator CurrentCoroutine = null;
	// Use this for initialization
	void Start () {
        LocationsToMoveTo = new Stack(2);
        MessageDispatcher.AddListener("MENU_MoveCameraTo", PanCamera);
	}

    void PanCamera(IMessage mess)
    {
        if (CurrentCoroutine != null)
        {
            LocationsToMoveTo.Push((int)(mess.Data));
        }
        else
        {
            CurrentCoroutine = MovementSpeed((int)(mess.Data));
            StartCoroutine(CurrentCoroutine);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            if(CurrentCoroutine != null)
            {
                LocationsToMoveTo.Push(0);
            }
            else
            {
                CurrentCoroutine = MovementSpeed(0);
                StartCoroutine(CurrentCoroutine);
            }
        }
    }

    IEnumerator MovementSpeed(int TargetIndex)
    {
        float time = 0.0f;
        Vector3 MovementVector = TargetLocations[TargetIndex].transform.position - this.gameObject.transform.position;
        Vector3 StartingPosition = this.gameObject.transform.position;
        Quaternion StartingRotation = this.gameObject.transform.rotation;
        if (MovementVector.magnitude > Mathf.Epsilon)
        {
            while (time < MovementTime)
            {
                this.gameObject.transform.rotation = 
                    Quaternion.Slerp(StartingRotation, TargetLocations[TargetIndex].transform.rotation, 
                    MovementCurve.Evaluate(time / MovementTime));
                this.gameObject.transform.position = MovementVector * MovementCurve.Evaluate(time / MovementTime) + StartingPosition;
                yield return null;
                time += Time.smoothDeltaTime;
            }
        }
        CleanUp();
    }

    void CleanUp()
    {
        CurrentCoroutine = null;

        while(LocationsToMoveTo.Count > 1)
        {
            LocationsToMoveTo.Pop();
        }
        if(LocationsToMoveTo.Count > 0)
        {
            CurrentCoroutine = MovementSpeed((int)LocationsToMoveTo.Pop());
            StartCoroutine(CurrentCoroutine);
        }
    }
}
