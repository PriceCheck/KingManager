using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpMover : MonoBehaviour {
    public AnimationCurve FadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    float totalAnimationTime = 0.8f;
    Vector3 StartingPoint = Vector3.zero;
    Vector3 EndingPoint;
    Color startingColor = Color.white;
    Text myText;
    float time;
    bool isInit = false;
   // IEnumerator TextAnimation = null;
    bool isRunningAnimation = false;
    Vector3 MovementVector;
    Color Fade; 
    void Awake()
    {
        init();
        
    }

    void Update()
    {
        if (isRunningAnimation)
        {
            myText.transform.position = StartingPoint + (MovementVector * (time / totalAnimationTime));
            Fade.a = FadeCurve.Evaluate(time / totalAnimationTime);
            myText.color = Fade;
            //yield return null;
            time += Time.smoothDeltaTime;
            if(time >= totalAnimationTime)
            {
                Fade.a = 0;
                myText.color = Fade;
                CleanUp();
            }
        }
        

    }

    void init()
    {
        if (!isInit)
        {
         //   SetActive(true);
            myText = GetComponent<Text>();
            StartingPoint = gameObject.transform.position;
            EndingPoint = StartingPoint + new Vector3(0, 2, 0);
            MovementVector = EndingPoint - StartingPoint;
            isInit = true;
            Fade = startingColor;
        }
    }

    public void RunAnimation(string text)
    {

        init();
        myText.text = text;
        time = 0;
        isRunningAnimation = true;

        /*if (!isRunningAnimation)
        {
            MovementVector = EndingPoint - StartingPoint;
            Fade = startingColor;
            
        }*/
    }

    void CleanUp()
    {
        isRunningAnimation = false;
    }
}
