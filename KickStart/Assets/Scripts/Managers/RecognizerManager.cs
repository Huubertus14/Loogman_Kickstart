using UnityEngine;
using UnityEngine.XR.WSA.Input;
using EnumStates;
using VrFox;

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
            if (GameManager.Instance.CurrentHandState == HandStates.NotVisible)
            {
                GameManager.Instance.InstrucionAmount += 3;
                //Debug.Log("Clicker is used");
            }
            else
            {
                GameManager.Instance.InstrucionAmount++;
              //  Debug.Log("Gesture is used");
            }
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
                GameManager.Instance.StartGame();
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
