using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;

        Player = Camera.main.gameObject;
    }

    [Header("References")]
    public GameObject Player;
    

    public GameObject CurrentPlayer
    {
        get { return Player; }
    }
}
