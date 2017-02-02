using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using com.ootii.Messages;

public class LevelTransition : MonoBehaviour {
    float TotalFadeInAnimationTime = 0.5f;
    float totalAnimationTime = 0.75f;
    IEnumerator currentAnimation;
    Image img;
	// Use this for initialization
	void Start () {
        MessageDispatcher.AddListener("LEVELS_StartLevelChange", StartLevelChange);
        img = this.GetComponent<Image>();
        currentAnimation = FadeIn();
        StartCoroutine(currentAnimation);
    }
	
    void StartLevelChange(IMessage mess)
    {
        if(currentAnimation == null)
        {
            currentAnimation = FadeOut((int)mess.Data);
            StartCoroutine(currentAnimation);
        }
    }

    IEnumerator FadeOut(int Scene)
    {
        float timer = 0;
        Color currentColor = img.color;
     
        while(timer < totalAnimationTime)
        {
            yield return null;
            timer += Time.smoothDeltaTime;
            currentColor.a = timer / totalAnimationTime;
            img.color = currentColor;
        }
        currentAnimation = null;
        if(Scene == -1)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(Scene, LoadSceneMode.Single);
        }
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForFixedUpdate();
        float timer = 0;
        Color currentColor = img.color;
        while (timer < TotalFadeInAnimationTime)
        {
            yield return null;
            timer += Time.smoothDeltaTime;
            currentColor.a = 1 - (timer / TotalFadeInAnimationTime);
            img.color = currentColor;
        }
        currentAnimation = null;

    }
    // Update is called once per frame
    void Update () {
		
	}
}
