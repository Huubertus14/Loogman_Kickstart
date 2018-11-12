using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFox;
using EnumStates;

public class DustyManager : MonoBehaviour
{
    public static DustyManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("Refs:")]
    public GameObject Head;
    public GameObject LeftArm;
    public GameObject RightArm;
    public GameObject Torso;
    public GameObject TextBillboard;

    private Animation ani;
    private DustyText dustyTextMessage;
    [HideInInspector]
    public AudioSource sourceAudio;

    float headSpinTimer;

    [Header("Values")]
    public Vector3 PositionAwayFromPlayer;
    private Vector3 goalPos;
    public DustyStates DustyState;

    [Header("Idle Values:")]
    public AnimationCurve YPosition;

    private float yPosTimer;
    private float yPosValue;

    [Header("Narrative:")]
    [SerializeField]
    public List<DustyTextFile> Messages = new List<DustyTextFile>();

    private float idleTextCounter, idleTextTimer;
    

    private void Start()
    {
        Messages.Clear();

        ani = Head.GetComponent<Animation>();
        dustyTextMessage = GetComponentInChildren<DustyText>();
        sourceAudio = GetComponentInChildren<AudioSource>();
       
        goalPos = Vector3.zero;
        DustyState = DustyStates.Idle;
        yPosTimer = 0;
        idleTextTimer = 0;
        idleTextCounter = Random.Range(8, 15);

        goalPos = Camera.main.transform.position + PositionAwayFromPlayer;

        //Add massages
        Messages.Add(new DustyTextFile("Hoi ik ben Dusty",4f, AudioSampleManager.Instance.DustyText[0]));
        Messages.Add(new DustyTextFile("Wat leuk dat je in de carwash bent", 4f, AudioSampleManager.Instance.DustyText[1]));
        Messages.Add(new DustyTextFile("Hopelijk zit je comfortabel", 4f, AudioSampleManager.Instance.DustyText[2]));
        Messages.Add(new DustyTextFile("Ik ben hier omdat ik je hulp nodig heb", 4f, AudioSampleManager.Instance.DustyText[3]));
        Messages.Add(new DustyTextFile("Er vliegen allemaal vogels in de wasstraat", 4f, AudioSampleManager.Instance.DustyText[4]));
        Messages.Add(new DustyTextFile("En dat willen wij niet", 4f, AudioSampleManager.Instance.DustyText[5]));
        Messages.Add(new DustyTextFile("Wij gaan ervoor zorgen dat de vogels niks meer vies maken", 4f, AudioSampleManager.Instance.DustyText[6]));
        Messages.Add(new DustyTextFile("Laten we eerst even trainen", 4f, AudioSampleManager.Instance.DustyText[7]));
        Messages.Add(new DustyTextFile("Voor je zie je een vogel", 4f, AudioSampleManager.Instance.DustyText[8]));
        Messages.Add(new DustyTextFile("Richt met je ogen op de vogel", 4f, AudioSampleManager.Instance.DustyText[9]));
        Messages.Add(new DustyTextFile("Goedzo! nu heeft vogel een luier om", 4f, AudioSampleManager.Instance.DustyText[10]));
        Messages.Add(new DustyTextFile("PATS", 4f, AudioSampleManager.Instance.DustyText[11]));
        Messages.Add(new DustyTextFile("Ju hebt nu een punt", 4f, AudioSampleManager.Instance.DustyText[12]));
        Messages.Add(new DustyTextFile("Probeer nu de andere vogels te raken", 4f, AudioSampleManager.Instance.DustyText[13]));
        Messages.Add(new DustyTextFile("Maar let op! er zijn ook al vogels die ene luier om hebben", 4f, AudioSampleManager.Instance.DustyText[14]));
        Messages.Add(new DustyTextFile("Als je deze raakt valt zijn luier af en krijg je straf punten!", 4f, AudioSampleManager.Instance.DustyText[15]));
        Messages.Add(new DustyTextFile("Doe goed je best!", 4f, AudioSampleManager.Instance.DustyText[16]));
        Messages.Add(new DustyTextFile("Je bent er helemaal klaar voor", 4f, AudioSampleManager.Instance.DustyText[17]));
        Messages.Add(new DustyTextFile("Wij rijden zo naar rechts", 4f, AudioSampleManager.Instance.DustyText[18]));
        Messages.Add(new DustyTextFile("Veel Succes!", 4f, AudioSampleManager.Instance.DustyText[19]));
       
    }

    private void Update()
    {
        switch (DustyState)
        {
            case DustyStates.Idle:
                goalPos = Camera.main.transform.position + PositionAwayFromPlayer;
                goalPos.y += yPosValue / 3;
                Idle();
                break;
            case DustyStates.Pointing:
                goalPos = Camera.main.transform.position + PositionAwayFromPlayer;
                Pointing();
                break;
            case DustyStates.Talking:
                goalPos = Camera.main.transform.position + PositionAwayFromPlayer;
                Talking();
                break;
            default:
                break;
        }

        //lerp to right position
        SetDustyPosition();
        HandleNarrative();
    }

    private void SetDustyPosition()
    {
        transform.position = Vector3.Lerp(transform.position, goalPos, Time.deltaTime * 5);
    }

    private void Idle()
    {
        headSpinTimer += Time.deltaTime;
        if (headSpinTimer > 15)
        {
            headSpinTimer = 0;
            ani.Play();
        }

        yPosTimer += Time.deltaTime;
        if (yPosTimer < 1)
        {
            yPosValue = YPosition.Evaluate(yPosTimer);
        }
        else
        {
            //Reset the curve timer
            yPosTimer = 0;
        }

        //When the list is empty some times add random text
        if (Messages.Count < 1 && dustyTextMessage.CanSayNextSentence)
        {
            //Add Random thing for dusty to say
            idleTextTimer += Time.deltaTime;
            if (idleTextTimer > idleTextCounter)
            {
                idleTextCounter = Random.Range(8, 15);
                idleTextTimer = 0;

                //add random thing
                Messages.Add(new DustyTextFile(GameManager.Instance.GetDustyQuote, Random.Range(3, 6), Random.Range(1, 10)));
            }
        }
    }

    private void Talking()
    {

    }

    public void Pointing()
    {

    }

    /// <summary>
    /// handles the text above Dusty, And says new thing when its done
    /// </summary>
    private void HandleNarrative()
    {
        if (Messages.Count < 1)
        {
            return;
        }
        if (dustyTextMessage.CanSayNextSentence)
        {
            dustyTextMessage.SayMessage(Messages[0]);
            Messages.Remove(Messages[0]);
        }
    }

    /// <summary>
    /// Call this to say an important message that needs to be shown first
    /// 
    /// 0 is most priority
    /// </summary>
    /// <param Text fiel etc="_file"></param>
    /// <param input Index="_index"></param>
    public void ImportantMessage(DustyTextFile _file, int _index)
    {
        if (_index < Messages.Count)
        {
            Messages.Insert(_index, _file);
            dustyTextMessage.SayMessage(Messages[0]);
            Messages.Remove(Messages[0]);
        }
        else
        {
            Messages.Add(_file);
        }
    }
}
