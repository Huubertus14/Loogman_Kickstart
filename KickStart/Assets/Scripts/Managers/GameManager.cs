﻿using System.Collections;
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
    public GestureImage TutorialThing;

    public GameObject[] GestureInstructions;

    [HideInInspector] //Bah
    public List<Renderer> Targets = new List<Renderer>();

    [Tooltip("The Object the player is currently looking at")]
    private GameObject hoverObject;

    [Header("Values")]
    public bool GameStarted;
    public bool GameOver;

    public float TimePlayed;


    [Space]
    public float BulletForce;

    [HideInInspector]
    public bool CanContinueToNExtGame;

    //Timer to run when the game is over and will reset
    private float GameOverTimer;

    private void Start()
    {
        GameStarted = false;
    }

    private void Update()
    {
        if (GameStarted)
        {
            if (TimePlayed <= 0)
            {
                GameStarted = false;
                TimeText.text = "";
                GameOver = true;
                ShowEndScore();
            }
            else
            {
                TimePlayed -= Time.deltaTime;
                SetTimeText();
            }
        }

        if (GameOver)
        {
            GameOverTimer += Time.deltaTime;
            if (GameOverTimer > 2)
            {
                CanContinueToNExtGame = true;
                TutorialThing.IsVisible = true;
                SetGestureInstructions(true);
            }
        }
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
        SetGestureInstructions(false);
    }

    public void SetGestureInstructions(bool _value)
    {
        foreach (var item in GestureInstructions)
        {
            item.SetActive(_value);
        }
    }

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

    public void SetScoreText()
    {
        ScoreText.text = "Score: " + Player.Score.ToString();
    }

    public void SetGarbageText()
    {
        GarbageText.text = "You've been hit by " + Player.HitByGarbage.ToString() + " Pieces of Garbage!";
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
