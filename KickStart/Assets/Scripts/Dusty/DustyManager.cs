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
    public GameObject Mouth;
    private Renderer dustyMouthRenderer;

    private DustyText dustyTextMessage;
    public DustyMouthSequence MouthSequence;
    private AudioSource sourceAudio;
    private Animator animator;

    float headSpinTimer;

    [Header("Values")]
    public Vector3 PositionAwayFromPlayer;
    private Vector3 goalPos;
    public Texture DefaultMouthTexture;
    
    [Header("Narrative:")]
    [SerializeField]
    public List<DustyTextFile> Messages = new List<DustyTextFile>();
    
    
    private void Start()
    {
        Messages.Clear();
        
        dustyTextMessage = GetComponentInChildren<DustyText>();
        sourceAudio = GetComponentInChildren<AudioSource>();
        dustyMouthRenderer = Mouth.GetComponentInChildren<Renderer>();
        MouthSequence = GetComponent<DustyMouthSequence>();

        SetDustyMouthTexture(DefaultMouthTexture);

        Debug.Log("Set mouth init sprite");

        animator = GetComponent<Animator>();

        animator.Play("Idle");

        goalPos = Camera.main.transform.position + PositionAwayFromPlayer;

        Transform[] _childObjects = GetComponentsInChildren<Transform>();
        foreach (var _child in _childObjects)
        {
            if (_child.GetComponent<MeshRenderer>())
            {
                _child.gameObject.AddComponent(typeof(MeshCollider));
                _child.gameObject.tag = "Dusty";
            }
        }
    }
    
    private void Update()
    {
        goalPos = Camera.main.transform.position + PositionAwayFromPlayer;

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

    public void SetDustyMouthTexture(Texture _texture)
    {
        dustyMouthRenderer.material.SetTexture("_MainTex", _texture);
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

    public void Wave()
    {
        animator.Play("Wave");
    }

    public void DustyHit()
    {

    }

    public AudioSource GetAudioSource => sourceAudio; 

}
