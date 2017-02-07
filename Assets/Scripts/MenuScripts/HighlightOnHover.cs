using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class HighlightOnHover : MonoBehaviour
{
    public AudioSource HighlightSFX;

    public Text[] TextToHighlight;
    public Image[] ImgToHighlight;
    public float AnimationTime = 0.5f;
    public Color HighlightColor;
    public bool SeperateTextHighlight = false;
    public Color HighlightTextColor;
    float time = 0;
    float DTmultiplier = 0;
    Color StartingColor;
    Color StartingTextColor;
    IEnumerator CurrentCoroutine;

    // Use this for initialization
    void Start()
    {
        if(TextToHighlight.Length > 0)
        {
            StartingTextColor = TextToHighlight[0].color;
            StartingTextColor.a = 1;
        }
        if(ImgToHighlight.Length > 0)
        {
            StartingColor = ImgToHighlight[0].color;
        }
      
        if(!SeperateTextHighlight)
        {
            HighlightTextColor = HighlightColor;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    void CleanUp()
    {
        CurrentCoroutine = null;
        time = Mathf.Clamp(time, 0, AnimationTime);
        SetColor(Vector4.Lerp(StartingColor, HighlightColor, time / AnimationTime));
    }


    void SetColor(Color input)
    {

        for (int i = 0; i < ImgToHighlight.Length; ++i)
        {
            ImgToHighlight[i].color = input;
        }
    }

    void SetColorText(Color input)
    {
        for (int i = 0; i < TextToHighlight.Length; ++i)
        {
            TextToHighlight[i].color = input;
        }
    }

    IEnumerator Animation()
    {
        
        while (time >= 0 && time <= AnimationTime)
        {
            
            yield return null;
            SetColor(Vector4.Lerp(StartingColor, HighlightColor, time / AnimationTime));
            SetColorText(Vector4.Lerp(StartingTextColor, HighlightTextColor, time / AnimationTime));
            time += Time.deltaTime * DTmultiplier;
        }
        CleanUp();
    }

    public void OnMouseExit()
    {
        DTmultiplier = -1;
        if (CurrentCoroutine == null)
        {
            CurrentCoroutine = Animation();
            StartCoroutine(CurrentCoroutine);
        }
    }

    public void OnMouseEnter()
    {
        DTmultiplier = 1;
        if (HighlightSFX && !HighlightSFX.isPlaying)
        {
            HighlightSFX.Play();
        }
        if (CurrentCoroutine == null)
        {
            CurrentCoroutine = Animation();
            StartCoroutine(CurrentCoroutine);
        }
    }
}
