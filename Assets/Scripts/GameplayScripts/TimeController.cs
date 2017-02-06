using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {
    public static TimeController instance;
    public static float SpeedMultiplier = 1;
    public static float DTthisFrame;
    public static float SmoothDTthisframe;
    float CurrentTime = 0;
    float timeInADay = 5;
    int month = 1;
    void Start()
    {
        instance = this;
    }

	// Update is called once per frame
	void Update () {
        DTthisFrame = SpeedMultiplier * Time.deltaTime;
        SmoothDTthisframe = SpeedMultiplier * Time.smoothDeltaTime;
       CurrentTime += DTthisFrame;
	}
}
