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

    private Animation ani;

    float headSpinTimer;

    [Header("Values")]
    public Vector3 PositionAwayFromPlayer;
    private Vector3 goalPos;
    public DustyStates DustyState;

    [Header("Idle Values:")]
    public AnimationCurve YPosition;

    private float yPosTimer;
    private float yPosValue;

    private void Start()
    {
        ani = Head.GetComponent<Animation>();
        goalPos = Vector3.zero;
        DustyState = DustyStates.Idle;
        yPosTimer = 0;
        goalPos = Camera.main.transform.position + PositionAwayFromPlayer;
    }

    private void Update()
    {
        //Set initial goal pos for this frame


        switch (DustyState)
        {
            case DustyStates.Idle:
                goalPos = Camera.main.transform.position + PositionAwayFromPlayer;
                goalPos.y += yPosValue/3;
                Idle();
                break;
            case DustyStates.Pointing:
                goalPos = Camera.main.transform.position + PositionAwayFromPlayer;
                break;
            case DustyStates.Talking:
                goalPos = Camera.main.transform.position + PositionAwayFromPlayer;
                break;
            default:
                break;
        }

        //lerp to right position
        SetDustyPosition();
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
            // goalPos.y += yPosValue * 10;
        }
        else
        {
            yPosTimer = 0;
        }

    }


}
