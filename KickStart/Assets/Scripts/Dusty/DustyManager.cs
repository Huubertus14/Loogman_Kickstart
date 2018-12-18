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
    public GameObject TextBillboard;
    
    private DustyText dustyTextMessage;
    [HideInInspector]
    public AudioSource sourceAudio;

    float headSpinTimer;

    [Header("Values")]
    public Vector3 PositionAwayFromPlayer;
    private Vector3 goalPos;
    
    [Header("Narrative:")]
    [SerializeField]
    public List<DustyTextFile> Messages = new List<DustyTextFile>();
    
    private void Start()
    {
        Messages.Clear();
        
        dustyTextMessage = GetComponentInChildren<DustyText>();
        sourceAudio = GetComponentInChildren<AudioSource>();
       
        goalPos = Vector3.zero;

        goalPos = Camera.main.transform.position + PositionAwayFromPlayer;

        /*
        //Add massages
        Messages.Add(new DustyTextFile("Hoi ik ben Dusty, de schoonmaak robot", 5f, AudioSampleManager.Instance.DustyText[0]));
        Messages.Add(new DustyTextFile("Wat leuk dat je bij Loogman in de carwash komt", 5f, AudioSampleManager.Instance.DustyText[1]));
        Messages.Add(new DustyTextFile("Hopelijk zit je comfortabel in de auto", 5f, AudioSampleManager.Instance.DustyText[2]));
        Messages.Add(new DustyTextFile("Maar ik ben hier omdat ik je hulp nodig heb, Loogman heeft een probleempje", 5f, AudioSampleManager.Instance.DustyText[4]));
        Messages.Add(new DustyTextFile("Er vliegen allemaal vogels in de wasstraat en ze maken alle auto's vies!", 5f, AudioSampleManager.Instance.DustyText[5]));
        Messages.Add(new DustyTextFile("Dat willen wij natuurlijk niet, want anders wordt jouw auto ook weer vies", 5f, AudioSampleManager.Instance.DustyText[6]));
        Messages.Add(new DustyTextFile("Wij gaan ervoor zorgen dat de vogels niet meer op de auto kunnen poepen door ze een luier om te doen", 5f, AudioSampleManager.Instance.DustyText[7]));
        Messages.Add(new DustyTextFile("Laten we eerst even trainen", 5f, AudioSampleManager.Instance.DustyText[8]));
  */
    }

    

    private void Update()
    {
        //lerp to right position
        SetDustyPosition();
        HandleNarrative();
    }

    private void SetDustyPosition()
    {
        transform.position = Vector3.Lerp(transform.position, goalPos, Time.deltaTime * 5);
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

    public DustyText GetDustyText
    {
        get { return dustyTextMessage; }
    }
}
