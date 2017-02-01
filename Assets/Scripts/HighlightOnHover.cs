using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightOnHover : MonoBehaviour
{
    public Text[] TextToHighlight;
    public Image[] ImgToHighlight;
    public float AnimationTime = 0.5f;
    public Color HighlightColor;
    float time = 0;
    float DTmultiplier = 0;
    Color StartingColor;
    IEnumerator CurrentCoroutine;

    // Use this for initialization
    void Start()
    {
        StartingColor = ImgToHighlight[0].color;
    }

    // Update is called once per frame
    void Update()
    {

    }


    void CleanUp()
    {
        CurrentCoroutine = null;

        Color CurrentColor = StartingColor;
        time = Mathf.Clamp(time, 0, AnimationTime);
        SetColor(Vector4.Lerp(StartingColor, HighlightColor, time / AnimationTime));
    }


    void SetColor(Color input)
    {
        for (int i = 0; i < TextToHighlight.Length; ++i)
        {
            TextToHighlight[i].color = input;
        }
        for (int i = 0; i < ImgToHighlight.Length; ++i)
        {
            ImgToHighlight[i].color = input;
        }
    }

    IEnumerator Animation()
    {
        Color CurrentColor = HighlightColor;
        
        while (time >= 0 && time <= AnimationTime)
        {
            yield return null;
            SetColor(Vector4.Lerp(StartingColor, HighlightColor, time / AnimationTime));
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
        if (CurrentCoroutine == null)
        {
            CurrentCoroutine = Animation();
            StartCoroutine(CurrentCoroutine);
        }
    }
}
