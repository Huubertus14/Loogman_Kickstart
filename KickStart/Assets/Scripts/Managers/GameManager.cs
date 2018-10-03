using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;

    }

    [Header("References")]
    public PlayerBehaviour Player;
    public GameObject Cursor;
    public Text ScoreText;
    public Text GarbageText;
    public Text TimeText;
    public Text EndScoreText;
    public GameObject PlayerCanvas;


    [Tooltip("The Object the player is currently looking at")]
    // [SerializeField]
    private GameObject hoverObject;

    [Header("Values")]
    public bool GameStarted;
    public string PlayerName;
    public int Score;
    public int HitByGarbage;
    public float TimePlayed;

    [Space]
    public float BulletForce;

    private void Start()
    {
        StartGame();
        PlayerName = "Loogman Develop";
        Score = 0;
        HitByGarbage = 0;
        SetScoreText();
    }

    private void Update()
    {
        if (TimePlayed > 180)
        {
            GameStarted = false;
            TimeText.text = "";
            ShowEndScore();
        }
        else
        {
            TimePlayed += Time.deltaTime;
            SetTimeText();
        }
    }

    /// <summary>
    /// Called when the game is about to start
    /// </summary>
    public void StartGame()
    {
        EndScoreText.text = "";
        GameStarted = true;
        TimePlayed = 0;
    }

    public void SetScoreText()
    {
        ScoreText.text = "Score: " + Score.ToString();
    }

    public void SetGarbageText()
    {
        GarbageText.text = "You've been hit by " + HitByGarbage.ToString() + " Pieces of Garbage!";
    }

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

    public void ShowEndScore()
    {
        EndScoreText.text = "You Got " + Score.ToString() + " Points\n And was Hit " + HitByGarbage.ToString()+" Times by Garbage";
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
