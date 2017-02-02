using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    RectTransform myPosition;
    VerticalLayoutGroup MyGroup;
    public LayoutElement element;
    public Image ScrollBackground;
    public Image[] ImgToFadeIn;
    public Text[] TextToFadeIn;
    public AnimationCurve SlideOnCurve = AnimationCurve.EaseInOut(0,1,1,0);
    public AnimationCurve OpenCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AudioSource ScrollOpenSFX;
    public AudioSource ScrollCloseSFX;

    Color targetColor = Color.black;
    float SlideOnTime = 0.3f;
    float ScrollOpenTime = 0.4f;
    float FadeInTime = 0.2f;

    float targetHeight = 210;
    float OffScreenPos = 600;
    int DirMultiplier = -1;

    int CurrentPhase = 0;

    IEnumerator CurrentAnimation = null;
	// Use this for initialization
	void Start () {
        
        myPosition = GetComponent<RectTransform>();
        MyGroup = GetComponent<VerticalLayoutGroup>();
        myPosition.anchoredPosition = new Vector2(OffScreenPos, 0);
        element.preferredHeight = 0;
        Color color = targetColor;
        color.a = 0;
        for (int i = 0; i < TextToFadeIn.Length; ++i)
        {
            TextToFadeIn[i].color = color;
        }
        for (int i = 0; i < ImgToFadeIn.Length; ++i)
        {
            ImgToFadeIn[i].color = color;
        }

    }
	
    void StartScrollOn()
    {
        DirMultiplier = 1;
        if (ScrollOpenSFX && !ScrollOpenSFX.isPlaying)
        {
            ScrollOpenSFX.Play();
        }
        if (CurrentAnimation == null)
        {
            CurrentPhase = 1;
            CurrentAnimation = MenuMoveOnAnimation();
            StartCoroutine(CurrentAnimation);
        }
    }

    void StartScrollOff()
    {
        DirMultiplier = -1;
        if (ScrollCloseSFX && !ScrollCloseSFX.isPlaying)
        {
            ScrollCloseSFX.Play();
        }
        if (CurrentAnimation == null)
        {
            CurrentPhase = 3;
            CurrentAnimation = MenuMoveOnAnimation();
            StartCoroutine(CurrentAnimation);
            
        }
    }

    IEnumerator MenuMoveOnAnimation()
    {
        float timer = 0;
        if(CurrentPhase == 3)
        {
            timer = FadeInTime;
        }

        while (CurrentPhase < 4 && CurrentPhase > 0)
        {
            yield return null;
            timer += Time.smoothDeltaTime * DirMultiplier;
            switch (CurrentPhase)
            {
                case (1):
                    {
                        if(ScrollSlideOn(timer))
                        {
                            CurrentPhase += DirMultiplier;
                            //Invert the timer, and then clamp it (ie: 1 - 1 = 0, 0 - 1 = -1 -> -(-1) = 1 )
                            timer = 0;
                            if(DirMultiplier == 1)
                            {
                                MyGroup.enabled = true;
                            }
                        }
                        break;
                    }
                case (2):
                    {
                        if (ScrollOpen(timer))
                        {
                            CurrentPhase += DirMultiplier;
                            MyGroup.enabled = false;
                            //Invert the timer, and then clamp it (ie: 1 - 1 = 0, 0 - 1 = -1 -> -(-1) = 1 )
                            if (DirMultiplier == -1)
                            {
                                timer = SlideOnTime;
                            }
                            else
                            {
                                timer = 0;
                            }
                        }
                        break;
                    }
                case (3):
                    {
                        if (TextFadeIn(timer))
                        {
                            CurrentPhase += DirMultiplier;
                            //Invert the timer, and then clamp it (ie: 1 - 1 = 0, 0 - 1 = -1 -> -(-1) = 1 )
                            if (DirMultiplier == -1)
                            {
                                timer = ScrollOpenTime;
                                MyGroup.enabled = true;
                            }
                            else
                            {
                                timer = 0;
                            }
                        }
                        break;
                    }
            }
            
        }
        CleanUp();
    }

    void CleanUp()
    {
        CurrentAnimation = null;
    }

    bool ScrollSlideOn (float time)
    {
        myPosition.anchoredPosition = new Vector2(OffScreenPos * SlideOnCurve.Evaluate(time/SlideOnTime), 0);
        if (time > SlideOnTime || time < 0)
        {
            return true;
        }
        return false;
    }

    bool ScrollOpen(float time)
    {
        element.preferredHeight = targetHeight * OpenCurve.Evaluate(time / ScrollOpenTime);
        if (time > ScrollOpenTime || time < 0)
        {
            return true;
        }
        return false;
    }
    bool TextFadeIn(float time)
    {
        Color color = targetColor;
        color.a = time / FadeInTime;
        for(int i = 0; i < TextToFadeIn.Length; ++i)
        {
            TextToFadeIn[i].color = color;
        }
        for (int i = 0; i < ImgToFadeIn.Length; ++i)
        {
            ImgToFadeIn[i].color = color;
        }
        if (time > FadeInTime || time < 0)
        {
            return true;
        }
        return false;
    }

    public void MenuButtonActivated()
    {
        if (DirMultiplier == -1)
        {
            StartScrollOn();
        }
        else
        {
            StartScrollOff();
        }
    }

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            MenuButtonActivated();
        }
	}
}
