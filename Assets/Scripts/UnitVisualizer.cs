using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitVisualizer : MonoBehaviour {
    TextUpMover myText;
    SpriteRenderer mySprite;
    public AnimationCurve FadeOutCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    public float FadeOutTime = 1.5f;
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextUpMover>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
           // Miss();
        }
    }

    void CreateMessage(string message)
    {
        myText.RunAnimation(message);
    }

    public void Attack(int Damage)
    {
        CreateMessage("Attacking for " + Damage + " Damage!");
    }

    public void TakeDamage(int Damage)
    {
        CreateMessage("Took " + Damage + " Damage");
    }

    public void Miss()
    {
        CreateMessage("Missed!");
    }
    public void Dead()
    {
        CreateMessage("Dead");
        gameObject.SetActive(true); enabled = true;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float time = 0;
        Color color = mySprite.color;
        while(time < FadeOutTime)
        {
            yield return null;
            color.a = 1 - (time / FadeOutTime);
            mySprite.color = color;
            time += Time.smoothDeltaTime;
        }
    }
}
