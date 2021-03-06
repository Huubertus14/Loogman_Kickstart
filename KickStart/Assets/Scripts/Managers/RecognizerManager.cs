﻿using EnumStates;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using VrFox;

public class RecognizerManager : MonoBehaviour
{
    public static RecognizerManager Instance;

    private GestureRecognizer recognizer;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap);
        recognizer.Tapped += TapHandler;
        recognizer.StartCapturingGestures();
    }

    /// <summary>
    /// Respond to Tap Input.
    /// </summary>
    private void TapHandler(TappedEventArgs obj)
    {
        //Switch to determine which game state there is eand to do someting with the hand
        switch (GameManager.Instance.GameState)
        {
            case GameStates.Playing:

                GameManager.Instance.Player.Shoot();
                break;
            case GameStates.Instructions:
                GameManager.Instance.Player.Shoot();
                break;
            case GameStates.GameEnd:
                if (GameManager.Instance.CanContinueNextGame)
                {
                    GameManager.Instance.ResetGame();

                    return;
                }
                break;
            case GameStates.Waiting:
                if (GameManager.Instance.GetHoverObject.GetComponent<StartButtonBehaviour>())
                {
                    if (GameManager.Instance.CurrentRound == Round.Intro)
                    {
                        GameManager.Instance.StartGame();
                    }
                    else if (GameManager.Instance.CurrentRound == Round.Round_1)
                    {
                        GameManager.Instance.StartRound_2();
                    }
                }

                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManager.Instance.GameState == GameStates.Waiting)
            {
                if (GameManager.Instance.CurrentRound == Round.Round_1)
                {
                    GameManager.Instance.StartRound_2();
                }
                else
                {
                    GameManager.Instance.StartGame();
                }
                return;
            }

            if (GameManager.Instance.GameState == GameStates.GameEnd)
            {
                if (GameManager.Instance.CanContinueNextGame)
                {
                    GameManager.Instance.ResetGame();
                    return;
                }
            }
        }
    }



}