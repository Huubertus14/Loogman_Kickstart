using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumStates;
using Greyman;
using Microsoft;

namespace EnumStates
{
    public enum GameStates
    {
        Playing,
        Instructions,
        GameEnd,
        Waiting
    }

    public enum DustyStates
    {
        Idle,
        Pointing,
        Talking
    }

    public enum Difficulty
    {
        Noob,
        Beginner,
        Normal,
        Hard
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
        public CrossHairTween CrossHairEffect;

        [Header("UI Elements:")]
        public Text ScoreText;
        public Text GarbageText;
        public Text TimeText;
        public Text EndScoreText;
        public UIFadeScript HighScoreFade;

        [HideInInspector]
        public List<Renderer> Targets = new List<Renderer>();

        [Tooltip("The Object the player is currently looking at")]
        private GameObject hoverObject;
        
        [Header("Material Colors:")]
        public Material Blue;
        public Material White;

        [Header("Values")]
        public GameStates gameState;
        public bool GameStarted;
        public bool GameOver;
        [Space]
        public float TimePlayed;
        public int InstrucionAmount;
        [Space]
        public float DurationToImpusle;
        public float DurationFromImpulse;
        
        private bool scoreTextFadedAway;
        private bool playerScoreOnRightPosition;
        private bool otherScoresShown;
        private bool otherScoreLerpIn;
        private GameObject playerEndScoreObject;

        [Header("Tutorial values")]
        public Text TutorialFeedbackText;
        public GameObject BoundaryIndicators;

        [Space]
        public ActivationLerp[] InstructionLerps;
        private float BeginTimer = 0;

        [HideInInspector]
        public float BulletForce;

        [HideInInspector]
        public bool CanContinueNextGame;
        
        //Test
        //Timer to run when the game is over and will reset
        private float GameOverTimer;

        private void Start()
        {
            BulletForce = 720;
            ResetGame();
            BoundaryIndicators.SetActive(true);
        }

        private void Update()
        {
            if (gameState == GameStates.Instructions) // do this when youare in the instructions
            {
                BeginTimer += Time.deltaTime;
                TutorialFeedbackText.text = "";
                if (BeginTimer > 3)
                {
                    SetAllInstructionsActive(true);
                    Instructions();
                }

                // Instructions();
            }
            if (gameState == GameStates.Playing)
            {
                Playing();
            }
            if (gameState == GameStates.Waiting)
            {
                //Wait for picture to find
            }
            if (gameState == GameStates.GameEnd)
            {
                EndGame();
            }
        }
        
        private void Instructions()
        {
            EndScoreText.text = "";

            if (InstrucionAmount == 0)
            {
                TutorialFeedbackText.text = "Click with the clicker!";
                BoundaryIndicators.SetActive(true);

                return;
            }

            //start the game from the gesture manager
            if (InstrucionAmount >= 1)
            {
                TutorialFeedbackText.text = "";
                SendTextMessage("Try to shoot as many birds as possible!", 12, Vector2.zero);

                //rempve arrows
                BoundaryIndicators.SetActive(false);


                StartGame();
                return;
            }
            InstrucionAmount = 0;
        }

        private void Playing()
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

            if (Input.GetKeyDown(KeyCode.O))
            {
                SetGameOver();
            }
        }

        private  void EndGame()
        {
            //Game has end
            GameOverTimer += Time.deltaTime;
            if (GameOverTimer > 3 && !scoreTextFadedAway)
            {
                scoreTextFadedAway = true;
                EndScoreText.text = "";

                ScoreManager.Instance.CreateAllScores(Player.Score);
                CrossHairEffect.SetActive(false, 0.9f);

                //Calculate the position of the player
                if (ScoreManager.Instance.GetPositionInTable(Player.Score) == 0)
                {
                    Debug.Log("Player is First");
                    playerEndScoreObject.GetComponentInChildren<Image>().color = Color.white;
                }
            }
            if (GameOverTimer > 4.5f && !playerScoreOnRightPosition)
            {
                playerScoreOnRightPosition = true;

                //Set Score thing on y pos
                playerEndScoreObject.transform.SetParent(Player.HighScoreObject.transform);
            }
            if (GameOverTimer > 5 && !otherScoresShown)
            {
                otherScoresShown = true;

                //Create other scores around
                GameObject[] OtherScores = new GameObject[5];

                switch (ScoreManager.Instance.GetPositionInTable(Player.Score))
                {
                    case 0:
                        //first place

                        //Get the 5 beneath the player
                        for (int i = 0; i < OtherScores.Length; i++)
                        {
                            OtherScores[i] = ScoreManager.Instance.CreateScoreBox(ScoreManager.Instance.AllScores[ScoreManager.Instance.GetPositionInAllScores(Player.Score) + i + 1]);

                            OtherScores[i].transform.SetParent(Player.HighScoreObject.transform);
                            OtherScores[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                            OtherScores[i].transform.localPosition = Vector3.zero;
                        }
                        break;
                    case 1:
                        //second place
                        OtherScores[0] = ScoreManager.Instance.CreateScoreBox(ScoreManager.Instance.AllScores[ScoreManager.Instance.GetPositionInAllScores(Player.Score) - 1]);
                        OtherScores[0].transform.SetParent(Player.HighScoreObject.transform);
                        OtherScores[0].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                        OtherScores[0].transform.localPosition = Vector3.zero;

                        playerEndScoreObject.transform.SetAsLastSibling();
                        //Get 1 above the player 4 beneath
                        for (int i = 1; i < OtherScores.Length; i++)
                        {
                            OtherScores[i] = ScoreManager.Instance.CreateScoreBox(ScoreManager.Instance.AllScores[ScoreManager.Instance.GetPositionInAllScores(Player.Score) + i + 1]);

                            OtherScores[i].transform.SetParent(Player.HighScoreObject.transform);
                            OtherScores[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                            OtherScores[i].transform.localPosition = Vector3.zero;
                        }
                        break;
                    case 2:
                        //third place
                        //get 2 above 3 beneath
                        for (int i = OtherScores.Length - 4; i >= 0; i--)
                        {
                            OtherScores[i] = ScoreManager.Instance.CreateScoreBox(ScoreManager.Instance.AllScores[ScoreManager.Instance.GetPositionInAllScores(Player.Score) - i - 1]);

                            OtherScores[i].transform.SetParent(Player.HighScoreObject.transform);
                            OtherScores[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                            OtherScores[i].transform.localPosition = Vector3.zero;
                        }

                        playerEndScoreObject.transform.SetAsLastSibling();

                        for (int i = 2; i < OtherScores.Length; i++)
                        {
                            OtherScores[i] = ScoreManager.Instance.CreateScoreBox(ScoreManager.Instance.AllScores[ScoreManager.Instance.GetPositionInAllScores(Player.Score) + i]);

                            OtherScores[i].transform.SetParent(Player.HighScoreObject.transform);
                            OtherScores[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                            OtherScores[i].transform.localPosition = Vector3.zero;
                        }
                        break;
                    case 3:
                        //third place

                        //3 above 2 beneath

                        for (int i = OtherScores.Length - 3; i >= 0; i--)
                        {
                            OtherScores[i] = ScoreManager.Instance.CreateScoreBox(ScoreManager.Instance.AllScores[ScoreManager.Instance.GetPositionInAllScores(Player.Score) - i - 1]);

                            OtherScores[i].transform.SetParent(Player.HighScoreObject.transform);
                            OtherScores[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                            OtherScores[i].transform.localPosition = Vector3.zero;
                        }

                        playerEndScoreObject.transform.SetAsLastSibling();

                        for (int i = 3; i < OtherScores.Length; i++)
                        {
                            OtherScores[i] = ScoreManager.Instance.CreateScoreBox(ScoreManager.Instance.AllScores[ScoreManager.Instance.GetPositionInAllScores(Player.Score) + i]);

                            OtherScores[i].transform.SetParent(Player.HighScoreObject.transform);
                            OtherScores[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                            OtherScores[i].transform.localPosition = Vector3.zero;
                        }
                        break;
                    case 4:
                        //second last place
                        //1 beneath 4 above
                        for (int i = OtherScores.Length - 2; i >= 0; i--)
                        {
                            OtherScores[i] = ScoreManager.Instance.CreateScoreBox(ScoreManager.Instance.AllScores[ScoreManager.Instance.GetPositionInAllScores(Player.Score) - i - 1]);

                            OtherScores[i].transform.SetParent(Player.HighScoreObject.transform);
                            OtherScores[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                            OtherScores[i].transform.localPosition = Vector3.zero;
                        }

                        //Set self
                        playerEndScoreObject.transform.SetAsLastSibling();

                        //find one below
                        OtherScores[4] = ScoreManager.Instance.CreateScoreBox(ScoreManager.Instance.AllScores[ScoreManager.Instance.GetPositionInAllScores(Player.Score) - 1]);
                        OtherScores[4].transform.SetParent(Player.HighScoreObject.transform);
                        OtherScores[4].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                        OtherScores[4].transform.localPosition = Vector3.zero;
                        break;
                    case 5:
                        //last place 
                        //5 above player

                        for (int i = OtherScores.Length - 1; i >= 0; i--)
                        {
                            OtherScores[i] = ScoreManager.Instance.CreateScoreBox(ScoreManager.Instance.AllScores[ScoreManager.Instance.GetPositionInAllScores(Player.Score) - i - 1]);

                            OtherScores[i].transform.SetParent(Player.HighScoreObject.transform);
                            OtherScores[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                            OtherScores[i].transform.localPosition = Vector3.zero;
                            playerEndScoreObject.transform.SetAsLastSibling();
                        }
                        break;
                    default:
                        //Middle pos
                        Debug.LogError("Not in range player position");

                        break;
                }

                foreach (var _score in OtherScores)
                {
                    _score.GetComponentInChildren<Text>().color = new Color(.9f, .9f, .9f, .8f);
                }

                Vector3 _xPosOffset = new Vector3(800, 0, 0);

                //Get array of all children of score
                PositionLerp[] _allScores = Player.HighScoreObject.GetComponentsInChildren<PositionLerp>();
                for (int i = 0; i < _allScores.Length; i++)
                {
                    _allScores[i].SetActivePosition(new Vector3(0, 55 - (44 * i), 0));

                    if (Random.Range(0, 10) % 2 == 0)
                    {
                        _xPosOffset = -_xPosOffset;
                    }

                    _allScores[i].SetInactivePosition(new Vector3(0, 55 - (44 * i), 0) + _xPosOffset);
                    if (_allScores[i] != playerEndScoreObject.GetComponent<PositionLerp>())
                    {
                        _allScores[i].GetRectTransform.localPosition = new Vector3(0, 55 - (44 * i), 0) + _xPosOffset;
                        _allScores[i].SetActive(false);
                    }
                }

                playerEndScoreObject.GetComponent<PositionLerp>().SetActive(true);
                //Fade in HighScoreText
                HighScoreFade.Active(true);
            }

            if (GameOverTimer > 6.5f && !otherScoreLerpIn)
            {
                otherScoreLerpIn = true;
                PositionLerp[] _allScores = Player.HighScoreObject.GetComponentsInChildren<PositionLerp>();
                for (int i = 0; i < _allScores.Length; i++)
                {
                    _allScores[i].SetActive(true, Random.Range(0.8f, 2.5f));
                }

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

            scoreTextFadedAway = false;
            otherScoresShown = false;
            otherScoreLerpIn = false;
            playerScoreOnRightPosition = false;

            //rempve arrows
            BoundaryIndicators.SetActive(false);

            gameState = GameStates.Instructions;
        }

        public void SetAllInstructionsActive(bool _value)
        {
            for (int i = 0; i < InstructionLerps.Length; i++)
            {
                InstructionLerps[i].SetActive(_value, 1.8f);
            }
            //Debug.Log("Show instructions");
        }

        /// <summary>
        /// Call this when the player is game over
        /// </summary>
        public void SetGameOver()
        {
            if (!GameStarted)
            {
                return;
            }
            GameStarted = false;
            TimeText.text = "";
            GameOver = true;
            gameState = GameStates.GameEnd;

            CrossHairEffect.SetActive(false, 2.2f);

            ShowEndScore();

            //Debug.Log("SetGameOver");
        }

        /// <summary>
        /// Called when the game is about to start
        /// </summary>
        public void StartGame()
        {
            if (GameStarted)
            {
                return;
            }

            Player.ResetPlayerValues();
            CanContinueNextGame = false;
            GameOver = false;
            EndScoreText.text = "";
            TutorialFeedbackText.text = "";
            GameStarted = true;
            TimePlayed = 180;
            GameOverTimer = 0;
            SetScoreText();

            SpawnManager.Instance.ResetTutorial();
            
            //remove all instructions
            gameState = GameStates.Playing;

            CrossHairEffect.SetActive(true, 1.4f);
            SetAllInstructionsActive(false);
            Player.SyncCarWashWithPlayer(0);
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

            TimeText.text = (1f / Time.deltaTime).ToString();
        }

        /// <summary>
        /// Show the total end score
        /// </summary>
        public void ShowEndScore()
        {
            //Debug.Log("Let Fade in");
            EndScoreText.text = "Your Score is:";

            playerEndScoreObject = Instantiate(ScoreManager.Instance.CreateScoreBox(Player.Score), Vector3.zero, Quaternion.identity);
            playerEndScoreObject.transform.SetParent(PlayerCanvas.transform);
            playerEndScoreObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            playerEndScoreObject.transform.localPosition = new Vector3(EndScoreText.transform.localPosition.x, EndScoreText.transform.localPosition.y - 100, EndScoreText.transform.localPosition.z);
            playerEndScoreObject.GetComponent<PositionLerp>().SetActivePosition(new Vector3(EndScoreText.transform.localPosition.x, EndScoreText.transform.localPosition.y - 100, EndScoreText.transform.localPosition.z));
            playerEndScoreObject.GetComponent<PositionLerp>().SetActive(true);
        }

        /// <summary>
        /// Call this to show a message to the player
        /// </summary>
        /// <param string of the message="_mes"></param>
        /// <param how long th message is in view="_dur"></param>
        /// <param any 2d offset  ="_offset"></param>
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
        
        /// <summary>
        /// Get a random quote 
        /// </summary>
        
        public Difficulty GetDiffictuly
        {
            get { return Player.PlayerLevel; }
        }
        
        #endregion
    }
}