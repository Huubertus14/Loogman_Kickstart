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
        if (GameManager.Instance.gameState == GameStates.GameEnd)
        {
            if (GameManager.Instance.CanContinueNextGame)
            {
                GameManager.Instance.ResetGame();
                return;
            }
        }

        if (GameManager.Instance.gameState == GameStates.Instructions)
        {
            GameManager.Instance.InstrucionAmount++;
            return;
        }

        //Only shoot in the gaming stage
        if (GameManager.Instance.gameState == GameStates.Playing)
        {
            
            if (dubbeltap < 0.4f && dubbeltap > 0.1f)
            {
                Debug.Log("Dubbel Tap");
            }
            dubbeltap = 0;

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
                GameManager.Instance.StartGame();
                return;
            }

            if (GameManager.Instance.gameState == GameStates.GameEnd)
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