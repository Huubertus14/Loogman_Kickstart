﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Tooltip("The Object the player is currently looking at")]
    [SerializeField]
    private GameObject hoverObject;

    [Header("Values")]
    public string PlayerName;
    public int Score;

    [Space]
    public float BulletForce;

    private void Start()
    {
        PlayerName = "Loogman Develop";
        Score = 0;
    }


    /// <summary>
    /// Called when the game is about to start
    /// </summary>
    public void StartGame()
    {

    }

    #region Property's

    public GameObject CurrentPlayer
    {
        get { return Player.gameObject; }
    }

    public GameObject GetHoverObject
    {
        get {
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
