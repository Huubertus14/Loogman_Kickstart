﻿using EnumStates;
using Greyman;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    public enum BirdType
    {
        Normal,
        Fat,
        Fast
    }

    public enum Round
    {
        Intro,
        Round_1,
        Round_2,
        Round_3,
        Score
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
        public GameObject Reticle;
        [Space]

        public GameObject PlayerCanvas;
        public GameObject GroundCanvas;

        public StartButtonBehaviour StartButton;

        [Space]
        public OffScreenIndicator Indicator;
        public MessageTextBehaviour messageText;
        public CrossHairTween CrossHairEffect;

        [Header("UI Elements:")]
        public Text ScoreText;
        public Text GarbageText;
        public Text TimeText;
        public Text EndScoreText;
        public Text ScoreFloorText;
        public UIFadeScript HighScoreFade;

        [Header("Material Colors:")]
        public Material Blue;
        public Material White;

        [Header("Values")]
        public GameStates GameState;
        public Round CurrentRound;
        public bool GameStarted;
        public bool GameOver;
        [Space]
        [Header("Time Values:")] // All the timing values in the carwash in seconds
        public float PreWashDuration;
        public float CarWashDuration;

        public float Round_1Duration;
        public float Round_2Duration;
        public float Round_3Duration;

        private GameObject playerEndScoreObject;
        private GameObject hoverObject;

        [Header("Tutorial values")]
        public Text TutorialFeedbackText;
        public GameObject BoundaryIndicators;

        [Space]
        public ActivationLerp[] InstructionLerps;

        private float bulletForce;

        [HideInInspector]
        public bool CanContinueNextGame;

        private void Start()
        {
            bulletForce = 720;
            ResetGame();
            BoundaryIndicators.SetActive(true);
        }

        private void Update()
        {
            switch (GameState)
            {
                case GameStates.Playing:
                    Playing();
                    break;
                case GameStates.Instructions:
                    //BeginTimer += Time.deltaTime;
                    //TutorialFeedbackText.text = "";
                    //if (BeginTimer > 3)
                    //{
                    //    SetAllInstructionsActive(true);
                    //    Instructions();
                    //}
                    break;
                case GameStates.GameEnd:
                    break;
                case GameStates.Waiting:
                    break;
                default:
                    break;
            }
        }
        

        /// <summary>
        /// Logic done when the game is playing
        /// </summary>
        private void Playing()
        {
            //Game is started
           
        }

        private IEnumerator StartTutorial()
        {
            Debug.Log("Starting the tutorial...");
            yield return new WaitForSeconds(0.5f);
            //Show UI to verify sight

            yield return new WaitForSeconds(1.5f);

            //let dusty say things



            CrossHairEffect.SetActive(true, 1.4f);
            yield return new WaitForSeconds(0.2f);

            GameState = GameStates.Playing;
            CurrentRound = Round.Round_1;
            yield return null;
        }

        /// <summary>
        /// Shows the score of the player and it's current rank in the high scores
        /// </summary>
        /// <returns></returns>
        private IEnumerator EndGame()
        {
            yield return new WaitForSeconds(3);
            EndScoreText.text = "";

            ScoreManager.Instance.CreateAllScores(Player.Score);
            CrossHairEffect.SetActive(false, 0.9f);

            //Calculate the position of the player
            if (ScoreManager.Instance.GetPositionInTable(Player.Score) == 0)
            {
                Debug.Log("Player is First");
                playerEndScoreObject.GetComponentInChildren<Image>().color = Color.white;
            }
            yield return new WaitForSeconds(1.5f);

            //Set Score thing on y pos
            playerEndScoreObject.transform.SetParent(Player.HighScoreObject.transform);
            yield return new WaitForSeconds(0.5f);

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
            yield return new WaitForSeconds(1.5f);

            _allScores = Player.HighScoreObject.GetComponentsInChildren<PositionLerp>();
            for (int i = 0; i < _allScores.Length; i++)
            {
                _allScores[i].SetActive(true, Random.Range(0.8f, 2.5f));
            }
            yield return null;
        }

        /// <summary>
        /// Reset the game and all its values to the begin state of the game
        /// </summary>
        public void ResetGame()
        {
            TutorialFeedbackText.text = "";
            EndScoreText.text = "";
            TimeText.text = "";
            ScoreText.text = "";
            ScoreFloorText.text = "Score:";
            GameStarted = false;

            //Set the duration of all the rounds
            Round_1Duration = PreWashDuration * 0.4f;
            Round_2Duration = CarWashDuration* 0.45f;
            Round_3Duration = CarWashDuration * 0.45f;

            //rempve arrows
            BoundaryIndicators.SetActive(false);
            StartButton.gameObject.SetActive(true);
            GameState = GameStates.Waiting;
            CurrentRound = Round.Intro;
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
            GameState = GameStates.GameEnd;
            StartCoroutine(EndGame());
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
            SetScoreText();

            StartButton.gameObject.SetActive(false);
            SpawnManager.Instance.ResetTutorial();

            GameState = GameStates.Instructions;
            CurrentRound = Round.Intro;

            //remove all instructions
            SetAllInstructionsActive(false);
        }

        /// <summary>
        /// Set the score text of the player
        /// </summary>
        public void SetScoreText()
        {
            ScoreText.text = "Score: " + Player.Score.ToString();
            ScoreFloorText.text = "Score: " + Player.Score.ToString();
        }

        /// <summary>
        /// Set the time text of the player
        /// </summary>
        public void SetTimeText()
        {
            Debug.Log("Commented");

            //int _min = (int)TimePlayed / 60;
            //int _sec = (int)TimePlayed % 60;

            //if (_sec < 10)
            //{
            //    TimeText.text = _min + ":0" + _sec;
            //}
            //else
            //{
            //    TimeText.text = _min + ":" + _sec;
            //}
        }

        public void HandleGaze(GameObject _gazeObject)
        {
            hoverObject = _gazeObject;
            if (_gazeObject.GetComponent<StartButtonBehaviour>())
            {
                StartButton.HoverEnter();
            }
            else
            {
                StartButton.HoverExit();
            }
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

        public GameObject CurrentPlayer => Player.gameObject;

        public float GetBulletForce => bulletForce;

        public Difficulty GetDiffictuly => Player.PlayerLevel;

        public GameObject GetHoverObject => hoverObject;

        #endregion
    }
}