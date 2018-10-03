using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEngine.XR.WSA.Input;

public class RecognizerManager : MonoBehaviour {

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
        if (!GameManager.Instance.GameStarted)
        {
            return;
        }
        GameManager.Instance.Player.Shoot();
    }

}
