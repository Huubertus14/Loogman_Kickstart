using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEngine.XR.WSA.Input;
using GamingStates;

public class RecognizerManager : MonoBehaviour {

    public static RecognizerManager Instance;

    private GestureRecognizer recognizer;

    private float LastGestureTimer;
    public float HintTimer;
    
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
        LastGestureTimer = 0;
        if (GameManager.Instance.gameState == GameStates.Instructions)
        {
            GameManager.Instance.InstrucionAmount++;
        }

        if (GameManager.Instance.gameState == GameStates.GameEnd)
        {
            if (GameManager.Instance.CanContinueToNExtGame)
            {
                GameManager.Instance.ResetGame();
            }
        }

        //Only shoot in the gaming stage
        if (GameManager.Instance.gameState == GameStates.Playing)
        {
            GameManager.Instance.Player.Shoot();
        }
    }

    private void Update()
    {
        LastGestureTimer += Time.deltaTime;
        if (LastGestureTimer > HintTimer)
        {
            //Debug.Log("Show Gestures Again");
        }
        else
        {
            //Debug.Log("Remove Gestures etc");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!GameManager.Instance.GameStarted)
            {
                if (GameManager.Instance.GameOver)
                {
                    if (GameManager.Instance.CanContinueToNExtGame)
                    {
                        GameManager.Instance.StartGame();
                        return;
                    }
                    return;
                }

                GameManager.Instance.StartGame();
                return;
            }
        }
    }
}
