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

        goalPos = Vector3.zero;
        DustyState = DustyStates.Idle;
        yPosTimer = 0;
        idleTextTimer = 0;
        idleTextCounter = Random.Range(8, 15);

        goalPos = Camera.main.transform.position + PositionAwayFromPlayer;

        //Add massages
        Messages.Add(new DustyTextFile("Hallo daar", 3, 5));
        Messages.Add(new DustyTextFile("Ik ben Dusty", 4, 5));
        Messages.Add(new DustyTextFile("En jij moet mij vandaag helpen", 5, 5));
        Messages.Add(new DustyTextFile("Loogman heeft een probleem", 4, 5));
        Messages.Add(new DustyTextFile("Er zijn allemaal vogels binnen de wasstraat", 5, 5));
        Messages.Add(new DustyTextFile("En zij maken alle schone auto's weer vies!", 4, 5));
        Messages.Add(new DustyTextFile("Nu is het aan jou om dat te voorkomen!", 6, 5));
        Messages.Add(new DustyTextFile("Anders word jouw auto straks ook weer vies", 3, 5));
        Messages.Add(new DustyTextFile("Je moet proberen om bij zo veel mogelijk\n vogels een luier om te schieten", 6, 5));
        Messages.Add(new DustyTextFile("Hoe doe je dat?", 2, 5));
        Messages.Add(new DustyTextFile("Simpel!", 1, 5));
        Messages.Add(new DustyTextFile("Volg de instructies die recht voor je staan", 3, 5));
        Messages.Add(new DustyTextFile("Of gebruik de clicker die is mee gegeven!", 4, 5));
        Messages.Add(new DustyTextFile("Voor elke vogel die je raakt krijg je een punt", 4, 5));
        Messages.Add(new DustyTextFile("En als je er een raakt die al een luier aan heeft...", 4, 5));
        Messages.Add(new DustyTextFile("Dan zal zijn luier afvallen en krijg je twee straf punten", 5, 5));
        Messages.Add(new DustyTextFile("Succes", 2, 5));
    }

    private void Update()
    {
        //Set initial goal pos for this frame
        if (Input.GetKeyDown(KeyCode.P))
        {
            Messages.Add(new DustyTextFile("Yeet " + Random.Range(10, 100), Random.Range(2, 4), Random.Range(1, 90)));
        }

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
