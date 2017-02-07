using UnityEngine;
using System.Collections;
using com.ootii.Messages;
using UnityEngine.UI;

public enum MenuButton
{
    Return,
    NewGame,
    LoadGame,
    Settings,
    Exit,
    StartGame,
    ExitGame
};

public class MainMenuButton : MonoBehaviour {
    public MenuButton button;
    public Text TextHighlight;
    public Image InscriptionHighlight;
    public AnimationCurve MoveUpMotion = AnimationCurve.EaseInOut(0,0,1,1);
    public AnimationCurve MoveDownMotion = AnimationCurve.EaseInOut(0,0,1,1);
    public Vector3 MoveDelta = new Vector3(0, 1, 0);
    public GameObject ObjectToMove;
    public AudioSource ButtonPickup;
    public AudioSource ButtonDrop;
    bool CanPlayAudio = false;
    bool HasGoneAbove = true;
    float ColorFillAnimationTime = 0.15f;

    public float ColorTime = 0;

    readonly float slowdownFactor = 2;
    AnimationCurve CurrentCurve;
    Vector3 StartingPosition;
    IEnumerator CurrentCoroutine = null;
    float time = 0;
    float AnimationTime = 0.5f;
    float DTmultiplier = -1;
    Color TextHighlightStart;
	// Use this for initialization
	void Start () {
        TextHighlightStart = TextHighlight.color;
        StartingPosition = ObjectToMove.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CleanUp()
    {
        CurrentCoroutine = null;
        CanPlayAudio = false;
        Color CurrentColor = TextHighlightStart;
        Vector3 finalPosition = transform.position;

        //Clamp Values
        finalPosition.y = Mathf.Clamp(finalPosition.y, StartingPosition.y, StartingPosition.y + MoveDelta.y);
        time = Mathf.Clamp(time, 0, AnimationTime);
        ColorTime = Mathf.Clamp(ColorTime, 0, ColorFillAnimationTime);
        //Set to end values
        InscriptionHighlight.fillAmount = ColorTime / ColorFillAnimationTime;
        CurrentColor.a = ColorTime / ColorFillAnimationTime;
        TextHighlight.color = CurrentColor;
    }

    IEnumerator Animation()
    {
        Color CurrentColor = TextHighlightStart;
        
        while(time >= 0 && time <= AnimationTime)
        {
            yield return null;
            InscriptionHighlight.fillAmount = ColorTime / ColorFillAnimationTime;
            CurrentColor.a = ColorTime / ColorFillAnimationTime;
            TextHighlight.color = CurrentColor;
            time += Time.smoothDeltaTime * DTmultiplier;
            ColorTime += Time.smoothDeltaTime * DTmultiplier;
            ColorTime = Mathf.Clamp(ColorTime, 0, ColorFillAnimationTime);
            MoveTowards(StartingPosition +
            MoveDelta * CurrentCurve.Evaluate(time / AnimationTime));
            if(DTmultiplier == -1)
            {
                ClackTest(ObjectToMove.transform.position);
            }
           
        }
        CleanUp();
    }
   
    

    void MoveTowards(Vector3 Destination)
    {
        ObjectToMove.transform.position = ((ObjectToMove.transform.position * (slowdownFactor - 1)) + Destination) / slowdownFactor;
    }

    void ClackTest(Vector3 Position)
    {
        if(!CanPlayAudio)
        {
            return;
        }
        if(Vector3.Distance(StartingPosition, Position) < 0.05)
        {
            if (HasGoneAbove && ButtonDrop && !ButtonDrop.isPlaying)
            {
                ButtonDrop.Play();
            }
            HasGoneAbove = false;
        }
        else
        {
            HasGoneAbove = true;
        }
      


    }

    public void IsHighlit()
    {
        DTmultiplier = -1;
        CanPlayAudio = true;
        CurrentCurve = MoveUpMotion;

        if (CurrentCoroutine == null)
        {
            CurrentCoroutine = Animation();
            StartCoroutine(CurrentCoroutine);
        }
        
    }

    public void IsUnHighlit()
    {
        DTmultiplier = 1;
        if (ButtonPickup && !ButtonPickup.isPlaying)
        {
            ButtonPickup.Play();
        }
        CurrentCurve = MoveDownMotion;
        if (CurrentCoroutine == null)
        {
            CurrentCoroutine = Animation();
            StartCoroutine(CurrentCoroutine);
        }
    }

    public void IsPushed()
    {
        CanPlayAudio = false;
        switch (button)
        {
            case (MenuButton.StartGame):
                {
                    MessageDispatcher.SendMessage(this, "LEVELS_StartLevelChange", 0,0);
                    break;
                }
            case (MenuButton.ExitGame):
                {
                  
                    MessageDispatcher.SendMessage(this, "LEVELS_StartLevelChange", -1, 0);
                    break;
                }

            case (MenuButton.NewGame):
                {
                    //Pan Up & Fade out
                    MessageDispatcher.SendMessage(this, "MENU_MoveCameraTo", (int)MenuButton.NewGame, 0);
                    break;
                }
            case (MenuButton.LoadGame):
                //Pan to the Loading screen
                MessageDispatcher.SendMessage(this, "MENU_MoveCameraTo", (int)MenuButton.LoadGame, 0);
                break;
            case (MenuButton.Settings):
                MessageDispatcher.SendMessage(this, "MENU_MoveCameraTo", (int)MenuButton.Settings, 0);
                break;
            case (MenuButton.Exit):
                {
                    MessageDispatcher.SendMessage(this, "MENU_MoveCameraTo", (int)MenuButton.Exit, 0);
                    break;
                }
            case (MenuButton.Return):
                {
                    MessageDispatcher.SendMessage(this, "MENU_MoveCameraTo", (int)MenuButton.Return, 0);
                    break;
                }
        }
    }
}
