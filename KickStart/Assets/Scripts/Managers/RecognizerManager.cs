using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEngine.XR.WSA.Input;
using EnumStates;

public class RecognizerManager : MonoBehaviour {

    public static RecognizerManager Instance;

    private GestureRecognizer recognizer;
    
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

        if (GameManager.Instance.gameState == GameStates.Instructions)
        {
            GameManager.Instance.InstrucionAmount++;
            return;
        }

        if (GameManager.Instance.gameState == GameStates.GameEnd)
        {
            if (GameManager.Instance.CanContinueToNExtGame)
            {
                GameManager.Instance.ResetGame();
                return;
            }
        }

        //Only shoot in the gaming stage
        if (GameManager.Instance.gameState == GameStates.Playing)
        {
            GameManager.Instance.Player.Shoot();
            return;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManager.Instance.gameState == GameStates.Instructions)
            {
                GameManager.Instance.InstrucionAmount++;
                return;
            }

            if (GameManager.Instance.gameState == GameStates.GameEnd)
            {
                if (GameManager.Instance.CanContinueToNExtGame)
                {
                    GameManager.Instance.ResetGame();
                    return;
                }
            }
        }
    }
}
