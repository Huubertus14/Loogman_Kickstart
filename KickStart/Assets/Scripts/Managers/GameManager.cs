using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GamingStates;

namespace GamingStates
{
    public enum GameStates
    {
        Playing,
        Instructions,
        GameEnd,
        Waiting
    }
}
public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;

    }

    [Header("References:s")]
    public PlayerBehaviour Player;
    public GameObject Cursor;

    public GameObject PlayerCanvas;
    public GestureImage TutorialThing;

    [Header("UI Elements:")]
    public Text ScoreText;
    public Text GarbageText;
    public Text TimeText;
    public Text EndScoreText;

    [Header("Instructions:")]
    public GameObject SightBoxes;
    public Text SightBoxesText;
    public GameObject GestureImage;
    public Text GestureImageText01;
    public Text GestureImageText02;
    public Image HandPlaceImage;

    [HideInInspector] //Bah
    public List<Renderer> Targets = new List<Renderer>();

    [Tooltip("The Object the player is currently looking at")]
    private GameObject hoverObject;

    [Header("Values")]
    public GameStates gameState;
    public bool GameStarted;
    public bool GameOver;
    [Space]
    public float TimePlayed;
    public int InstrucionAmount;

    [HideInInspector]
    public float BulletForce;

    [HideInInspector]
    public bool CanContinueToNExtGame;

    //Timer to run when the game is over and will reset
    private float GameOverTimer;

    private void Start()
    {
        BulletForce = 240;
        GameStarted = false;
        gameState = GameStates.Instructions;
    }

    private void Update()
    {
        if (gameState == GameStates.Instructions)
        {
            //Wait for instructions
            if (InstrucionAmount == 0)
            {
                //SHow one thing  //The bounding Boxes
                SightBoxes.SetActive(true);
                SightBoxesText.gameObject.SetActive(true);
                GestureImage.SetActive(true);
                GestureImageText01.gameObject.SetActive(false);
                GestureImageText02.gameObject.SetActive(false);
                HandPlaceImage.gameObject.SetActive(false);
            }
            if (InstrucionAmount == 1)
            {
                //Show gesture thing
                SightBoxes.SetActive(false);
                SightBoxesText.gameObject.SetActive(false);
                GestureImage.SetActive(true);
                GestureImageText01.gameObject.SetActive(true);
                GestureImageText02.gameObject.SetActive(true);
                HandPlaceImage.gameObject.SetActive(true);
            }
            if (InstrucionAmount == 2)
            {
                SightBoxes.SetActive(false);
                SightBoxesText.gameObject.SetActive(false);

                GestureImage.SetActive(false);
                GestureImageText01.gameObject.SetActive(false);
                GestureImageText02.gameObject.SetActive(false);
                HandPlaceImage.gameObject.SetActive(false);
                StartGame();
            }

            //start the game from the gesture manager
        }
        if (gameState == GameStates.Playing)
        {
            //Game is started
            if (TimePlayed <= 0)
            {
                SetGameOver();
            }
            else
            {
                TimePlayed -= Time.deltaTime;
                SetTimeText();
            }
        }
        if (gameState == GameStates.Waiting)
        {
            //Wait for something
        }
        if (gameState == GameStates.GameEnd)
        {
            //Game has end
            GameOverTimer += Time.deltaTime;
            if (GameOverTimer > 2)
            {
                CanContinueToNExtGame = true;
                TutorialThing.IsVisible = true;
                InstrucionAmount = 0;
            }
        }
        
    }

    /// <summary>
    /// Call this when the player is game over
    /// </summary>
    public void SetGameOver()
    {
        GameStarted = false;
        TimeText.text = "";
        GameOver = true;
        ShowEndScore();
    }

    /// <summary>
    /// Called when the game is about to start
    /// </summary>
    public void StartGame()
    {
        Player.ResetPlayerValues();
        CanContinueToNExtGame = false;
        GameOver = false;
        EndScoreText.text = "";
        GameStarted = true;
        TimePlayed = 180;
        GameOverTimer = 0;
        SetScoreText();

        //remove all instructions
        gameState = GameStates.Playing;
    }
    

    //Does not work right now
    public bool IsSeeingAnEnemy()
    {
        if (Targets.Count < 1)
        {
            return false;
        }
        for (int i = 0; i < Targets.Count; i++)
        {
            if (!Targets[i])
            {
                Targets.RemoveAt(i);
            }
            if (Targets[i].isVisible)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Set the score text of the player
    /// </summary>
    public void SetScoreText()
    {
        ScoreText.text = "Score: " + Player.Score.ToString();
    }

    /// <summary>
    /// Set garbage text, Might be obsolete
    /// </summary>
    public void SetGarbageText()
    {
        GarbageText.text = "You've been hit by " + Player.HitByGarbage.ToString() + " Pieces of Garbage!";
    }

    /// <summary>
    /// Set the time text of the player
    /// </summary>
    public void SetTimeText()
    {
        int _min = (int)TimePlayed / 60;
        int _sec = (int)TimePlayed % 60;

        if (_sec < 10)
        {
            TimeText.text = _min + ":0" + _sec;
        }
        else
        {
            TimeText.text = _min + ":" + _sec;
        }
    }

    /// <summary>
    /// Show the total end score
    /// </summary>
    public void ShowEndScore()
    {
        EndScoreText.text = "You Got " + Player.Score.ToString() + " Points!";
    }

    #region Property's

    public GameObject CurrentPlayer
    {
        get { return Player.gameObject; }
    }

    public GameObject GetHoverObject
    {
        get
        {
            if (hoverObject)
            {
                return hoverObject;
            }
            else
            {
                Debug.LogError("No Current Hovering Object!");
                return null;
            }
        }
    }

    public void SetHoverObject(GameObject _hoverObject)
    {
        hoverObject = _hoverObject;
    }
    #endregion
}
