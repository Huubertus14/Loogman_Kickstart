using UnityEngine;
using UnityEngine.XR.WSA.Input;
using EnumStates;
using VrFox;

public class RecognizerManager : MonoBehaviour {
    
    public static RecognizerManager Instance;

    private GestureRecognizer recognizer;
    
    private float dubbeltap;
    
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
                if (dubbeltap < 0.4f && dubbeltap > 0.1f)
                {
                    Debug.Log("Dubbel Tap");
                }
                dubbeltap = 0;

                GameManager.Instance.Player.Shoot();
                break;
            case GameStates.Instructions:
                GameManager.Instance.InstrucionAmount++;
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
                    GameManager.Instance.StartGame();
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
            if (GameManager.Instance.GameState == GameStates.Instructions)
            {
                GameManager.Instance.StartGame();
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

        dubbeltap += Time.deltaTime;
    }

    

}