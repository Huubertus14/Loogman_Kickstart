using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using EnumStates;
using Greyman;
using VrFox;

namespace EnumStates
{
    public enum GameStates
    {
        Playing,
        Instructions,
        GameEnd,
        Waiting
    }

    public enum HandStates
    {
        Visible,
        NotVisible,
        Select,
        Observing,
        Release
    }

    public enum DustyStates
    {
        Idle,
        Pointing,
        Talking
    }
}

namespace VrFox
{
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
        public OffScreenIndicator Indicator;
        public MessageTextBehaviour messageText;

        [Header("UI Elements:")]
        public Text ScoreText;
        public Text GarbageText;
        public Text TimeText;
        public Text EndScoreText;

        [HideInInspector] //Bah
        public List<Renderer> Targets = new List<Renderer>();

        [Tooltip("The Object the player is currently looking at")]
        private GameObject hoverObject;

        [Header("Values")]
        public GameStates gameState;
        public HandStates CurrentHandState;
        public bool GameStarted;
        public bool GameOver;
        [Space]
        public float TimePlayed;
        public int InstrucionAmount;

        [Header("Tutorial values")]
        public Text TutorialFeedbackText;
        public Image HandPlaceBox;
        public GameObject GestureAnimation;
        public GameObject BoundaryIndicators;

        private float instructionTimer;
        private float instructionCounter;

        [HideInInspector]
        public float BulletForce;

        [HideInInspector]
        public bool CanContinueToNExtGame;


        //Test
        //Timer to run when the game is over and will reset
        private float GameOverTimer;

        private void Start()
        {
            instructionTimer = 5.5f;

            CurrentHandState = HandStates.NotVisible;
            BulletForce = 240 * 3;
            ResetGame();
        }

        private void Update()
        {

            if (gameState == GameStates.Instructions) // do this when youare in the instructions
            {
                Instructions();
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
                    if (TimePlayed < 170)
                    {
                        TutorialFeedbackText.text = "";
                    }
                    if (Player.Score > 3)
                    {
                        TimePlayed -= Time.deltaTime;
                        SetTimeText();
                    }
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

        private void Instructions()
        {
            EndScoreText.text = "";
            if (CurrentHandState == HandStates.NotVisible)
            {
                TutorialFeedbackText.text = "Hold your hand in front of you";
            }
            if (InstrucionAmount == 0)
            {
                //Check if the hannd is visible
                if (CurrentHandState == HandStates.NotVisible)
                {
                    TutorialFeedbackText.text = "Hold your index finger in the box";
                    HandPlaceBox.gameObject.SetActive(true);
                    GestureAnimation.SetActive(false);
                    //Show visualBox
                    BoundaryIndicators.SetActive(true);
                }
                else
                {
                    TutorialFeedbackText.text = "Try to mimic the gesture";
                    HandPlaceBox.gameObject.SetActive(true);

                    GestureAnimation.SetActive(true);

                    BoundaryIndicators.SetActive(true);
                }

                return;
            }

            if (InstrucionAmount == 1)
            {
                instructionCounter += Time.deltaTime;
                //Check if the hannd is visible
                if (CurrentHandState == HandStates.NotVisible)
                {
                    TutorialFeedbackText.text = "Hold your index finger in the box";
                    HandPlaceBox.gameObject.SetActive(true);
                    GestureAnimation.SetActive(false);
                    InstrucionAmount = 0;
                    instructionCounter = 0;
                    //Show visualBox
                    BoundaryIndicators.SetActive(false);
                }
                else
                {
                    TutorialFeedbackText.text = "Great! Try it again!";
                    HandPlaceBox.gameObject.SetActive(true);
                    GestureAnimation.SetActive(true);

                    BoundaryIndicators.SetActive(false);

                    if (instructionCounter > instructionTimer)
                    {
                        instructionCounter = 0;
                        InstrucionAmount = 0;
                        BoundaryIndicators.SetActive(true);
                    }
                }

                return;
            }

            //start the game from the gesture manager
            if (InstrucionAmount >= 2)
            {
                TutorialFeedbackText.text = "Try to shoot as many Birds as possible!";
                //remove boxes
                GestureAnimation.SetActive(false);
                HandPlaceBox.gameObject.SetActive(false);

                //rempve arrows
                BoundaryIndicators.SetActive(false);


                StartGame();
                return;
            }
        }

        public void ResetGame()
        {
            TutorialFeedbackText.text = "";
            EndScoreText.text = "";
            TimeText.text = "";
            ScoreText.text = "";
            InstrucionAmount = 0;
            GameStarted = false;
            gameState = GameStates.Instructions;
        }

        /// <summary>
        /// Call this when the player is game over
        /// </summary>
        public void SetGameOver()
        {
            GameStarted = false;
            TimeText.text = "";
            GameOver = true;
            gameState = GameStates.GameEnd;

            Invoke("SetGestureActive", 3f);

            //Destroy all birds

            ShowEndScore();
        }

        private void SetGestureActive()
        {
            GestureAnimation.SetActive(true);
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

        public void SendTextMessage(string _mes, float _dur, Vector2 _offset)
        {
            messageText.Message(_mes, _dur, _offset);
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

        public void SetHandState(HandStates _state)
        {
            CurrentHandState = _state;
        }

        public string GetDustyQuote
        {
            get
            {
                string[] _quote = new string[] {
                    "Biem!",
                    "Is nou eigenlijk\n Kikker of Kinker?" ,
                    "Loogman best Man",
                    "Kobe",
                    "Yeet",
                    "WOW",
                    "Merci for Waluigi",
                    "Ik wist niet dat je Loog Man",
                    "TOTO - Africa",
                    "Bless the rains down in Africa",
                    "Tututututu",
                    "Ik wil Kaas",
                    "WAAAAAAAAAA",
                    "REEEEEEEEEEEEEEEEEEEEEEEE",
                    "@@@@@@@@@@@@@@@@@@@@@@@@@@",
                    "Alexa, play Despacito",
                "( ._. )",
                "ಠ_ಠ",
                "(╯°□°）╯︵ ┻━┻"
                };

                int _x = Random.Range(0, _quote.Length);

                return _quote[_x];
            }

        }
        #endregion
    }
}