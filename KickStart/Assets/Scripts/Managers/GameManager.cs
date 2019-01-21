using EnumStates;
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
        //public CrossHairTween CrossHairEffect;

        [Header("UI Elements:")]
        public Text ScoreText;
        public Text GarbageText;
        public Text TimeText;
        public Text EndScoreText;
        public Text ScoreFloorText;
        public Text AccuracyText;
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

        private float introDuration;
        private float round_1Duration;
        private float round_2Duration;
        private float round_3Duration;

        private float currentTimer;


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
        public bool FirstBirdHit = false;
        public bool SecondBirdHit = false;
        private bool timeReminder30, timeReminder60;


        private void Start()
        {
            bulletForce = 720;
            ResetGame();
            BoundaryIndicators.SetActive(true);
        }

        private float startTimer;

        private void Update()
        {
            switch (GameState)
            {
                case GameStates.Playing:
                    Playing();
                    SetScoreText();
                    SetAccuracyText();
                    break;
                case GameStates.Instructions:
                    SetScoreText();
                    currentTimer += Time.deltaTime;
                    if (currentTimer > introDuration)
                    {
                        //Intro done, start round one
                        currentTimer = 0;
                        StartRound_1();

                        //Stop the current running coroutine
                        StopCoroutine(StartTutorial());
                    }
                    TimeText.text = "";
                    ScoreText.text = "";
                    ScoreFloorText.text = "";
                    AccuracyText.text = "";
                    break;
                case GameStates.GameEnd:
                    break;
                case GameStates.Waiting:
                    if (CurrentRound == Round.Intro)
                    {
                        startTimer += Time.deltaTime;
                        if (startTimer > 9)
                        {
                            startTimer = 0;

                            //Say Look down
                            DustyManager.Instance.Messages.Add(new DustyTextFile("Kijk naar beneden", 5f, AudioSampleManager.Instance.DustyVoorWas[0]));
                        }
                    }
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
            switch (CurrentRound) //DO logic depending on current round
            {
                case Round.Intro:
                    Debug.LogError("Game is in play state");
                    break;
                case Round.Round_1:
                    if (currentTimer > round_1Duration)
                    {
                        SendTextMessage("Drive to the next carwash!", 4f, Vector2.zero);

                        //Wait unitl the player start the game again
                        SetCarWashPause();

                        currentTimer = 0;
                    }
                    break;
                case Round.Round_2:
                    if (currentTimer > round_2Duration)
                    {
                        SendTextMessage("Starting Round 3", 4f, Vector2.zero);
                        DustyManager.Instance.Messages.Add(new DustyTextFile("Je bent nu aan het einde van ronde 3", 5, AudioSampleManager.Instance.DustyRonde02[2]));
                        CurrentRound = Round.Round_3;
                        currentTimer = 0;
                    }
                    break;
                case Round.Round_3:
                    if (currentTimer > round_3Duration)
                    {
                        DustyManager.Instance.Messages.Add(new DustyTextFile("Je bent nu bij het einde", 5, AudioSampleManager.Instance.DustyMisc[0]));
                        SendTextMessage("Game Over", 4f, new Vector2(0, -40));
                        CurrentRound = Round.Score;
                        currentTimer = 0;
                        SetGameOver();
                    }

                    if (round_3Duration - currentTimer < 60 && !timeReminder60)
                    {
                        timeReminder60 = true;
                        DustyManager.Instance.Messages.Add(new DustyTextFile("nog 60 seconde!", 5, AudioSampleManager.Instance.DustyTimeReminder[0]));
                    }
                    else if (round_3Duration - currentTimer < 30 && !timeReminder30)
                    {
                        timeReminder30 = true;
                        DustyManager.Instance.Messages.Add(new DustyTextFile("nog 30 seconde!", 5, AudioSampleManager.Instance.DustyTimeReminder[1]));
                    }
                    break;
                case Round.Score:
                    Debug.LogError("Game is in play state");
                    break;
                default:
                    break;
            }
            currentTimer += Time.deltaTime;
        }
        
        private IEnumerator StartTutorial()
        {
            Debug.Log("Starting the tutorial...");
            SpawnManager.Instance.CurrentBirdCount = 0;
            yield return new WaitForSeconds(0.5f);
            //Show UI to verify sight
            DustyManager.Instance.Messages.Add(new DustyTextFile("Hoi in ben Dusty", 5f, AudioSampleManager.Instance.DustyVoorWas[1]));
            DustyManager.Instance.Messages.Add(new DustyTextFile("Wat leuk dat je bij Loogman in de wasstraat komt", 5f, AudioSampleManager.Instance.DustyVoorWas[2])); //Insert right texture amount
            DustyManager.Instance.Messages.Add(new DustyTextFile("Hopelijk zit je comfortabel in de auto", 5f, AudioSampleManager.Instance.DustyVoorWas[3]));
            SetAllInstructionsActive(true);

            yield return new WaitForSeconds(1.5f);

           DustyManager.Instance.PlayAnimation("Welcome");

            //let dusty say things
            DustyManager.Instance.Messages.Add(new DustyTextFile("Ik heb je hulp nodig", 5f, AudioSampleManager.Instance.DustyVoorWas[5]));
            DustyManager.Instance.Messages.Add(new DustyTextFile("Er zijn allemaal vogels in de wasstraat", 5f, AudioSampleManager.Instance.DustyVoorWas[6]));
            DustyManager.Instance.Messages.Add(new DustyTextFile("En dat willen wij niet", 5f, AudioSampleManager.Instance.DustyVoorWas[7]));
            DustyManager.Instance.Messages.Add(new DustyTextFile("We gaan een luier omdoen bij de vogels", 5f, AudioSampleManager.Instance.DustyVoorWas[8]));
            DustyManager.Instance.Messages.Add(new DustyTextFile("Laten we eerst even trainen", 5f, AudioSampleManager.Instance.DustyVoorWas[9]));

            yield return new WaitUntil(() => DustyManager.Instance.Messages.Count < 1);
            GameState = GameStates.Instructions;
            SpawnManager.Instance.SpawnBirdInFrontOfPlayer();
            DustyManager.Instance.Messages.Add(new DustyTextFile("Voor je zie je nu een vogel", 5f, AudioSampleManager.Instance.DustyRonde01[1]));
            DustyManager.Instance.Messages.Add(new DustyTextFile("Richt met je ogen op de vogel", 5f, AudioSampleManager.Instance.DustyRonde01[2]));

            //Loop clikcer thing?

            yield return new WaitUntil(() => FirstBirdHit);
            DustyManager.Instance.Messages.Clear();
            DustyManager.Instance.Messages.Add(new DustyTextFile("Pats!", 5f, AudioSampleManager.Instance.DustyRonde01[3]));
            DustyManager.Instance.Messages.Add(new DustyTextFile("Nu heeft de vogel een luier om!", 5f, AudioSampleManager.Instance.DustyRonde01[4]));
            DustyManager.Instance.Messages.Add(new DustyTextFile("Je hebt nu een punt!", 5f, AudioSampleManager.Instance.DustyRonde01[5]));

            yield return new WaitUntil(() => DustyManager.Instance.Messages.Count < 1);
            SpawnManager.Instance.SpawnBirdInFrontOfPlayer();
            DustyManager.Instance.Messages.Add(new DustyTextFile("Probeer nu de andere vogel ook te raken", 5f, AudioSampleManager.Instance.DustyRonde01[6]));

            //Loop Clicker thing?

            yield return new WaitUntil(() => SecondBirdHit);
            DustyManager.Instance.Messages.Clear();
            DustyManager.Instance.Messages.Add(new DustyTextFile("Pats!", 5f, AudioSampleManager.Instance.DustyRonde01[3]));
            DustyManager.Instance.Messages.Add(new DustyTextFile("Je bent er nu helemaal klaar voor!", 5f, AudioSampleManager.Instance.DustyRonde01[9]));


            yield return new WaitUntil(() => DustyManager.Instance.Messages.Count < 1);
            SetAllInstructionsActive(false);
            currentTimer = 0;
            StartRound_1();

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
            //CrossHairEffect.SetActive(false, 0.9f);

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
            ScoreFloorText.text = "";
            AccuracyText.text = "";
            GameStarted = false;

            FirstBirdHit = false;
            SecondBirdHit = false;

            //Set the duration of all the rounds
            introDuration = PreWashDuration * 0.6f;
            round_1Duration = PreWashDuration * 0.4f;
            round_2Duration = CarWashDuration * 0.45f;
            round_3Duration = CarWashDuration * 0.45f;

            timeReminder30 = false;
            timeReminder60 = false;

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
            //CrossHairEffect.SetActive(false, 2.2f);

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

            GameState = GameStates.Instructions;
            CurrentRound = Round.Intro;

            StartCoroutine(StartTutorial());

            //remove all instructions
            SetAllInstructionsActive(false);
        }

        /// <summary>
        /// Set the score text of the player
        /// </summary>
        public void SetScoreText()
        {
            ScoreText.text = "Score: " + Player.Score.ToString();
            ScoreFloorText.text = "Score: " + Player.Score.ToString() + "   Birds hit: " + Player.HitCount.ToString() + "   Diapers shot: " + Player.ShootCount.ToString();
        }

        /// <summary>
        /// Set the Accuracy of the player, if ShootCount exceeds 0.
        /// </summary>
        private void SetAccuracyText()
        {
            if (Player.ShootCount > 0)
            {
                float Accuracy = (Player.HitCount / Player.ShootCount) * 100;
                AccuracyText.text = "Accuracy: " + Accuracy.ToString() + "%";
            }
            else
            {
                AccuracyText.text = "Accuracy: " + "100" + "%";
            }
        }

        /// <summary>
        /// Called when the prewash is done and the player needs to drive to the second carwash
        /// </summary>
        private void SetCarWashPause()
        {
            GameState = GameStates.Waiting;

            //enable start button
            StartButton.gameObject.SetActive(true);
            DustyManager.Instance.Messages.Add(new DustyTextFile("We zijn aan het eind van de deze wasstraat gekomen", 5, AudioSampleManager.Instance.DustyRonde01[8]));
            DustyManager.Instance.Messages.Add(new DustyTextFile("Rij nu naar de rechts", 5, AudioSampleManager.Instance.DustyRonde01[12]));
            DustyManager.Instance.Messages.Add(new DustyTextFile("Als je bij de wasstraat bent kijk dan weer naar beneden", 5, AudioSampleManager.Instance.DustyRonde01[13]));
            //Remove all birds
            SpawnManager.Instance.ClearAllBirds();
        }

        /// <summary>
        /// Called to start the first round afther the tutorial
        /// </summary>
        private void StartRound_1()
        {
            if (GameState == GameStates.Playing)
            {
                return;
            }
            DustyManager.Instance.Messages.Clear();
            DustyManager.Instance.Messages.Add(new DustyTextFile("De eerste ronde gaat beginnen", 5, AudioSampleManager.Instance.DustyRonde01[0]));
            hoverObject = null;
            //CrossHairEffect.SetActive(true, 1.4f);
            GameState = GameStates.Playing;
            CurrentRound = Round.Round_1;
            SendTextMessage("Starting Round 1", 4f, Vector2.zero);
            Debug.Log("Start round 01");

            Player.Score = 0;
            Player.ShootCount = 0;
            Player.HitCount = 0;
        }

        /// <summary>
        /// Called when the second round needs to start. The player must drive to the next carwash
        /// </summary>
        public void StartRound_2()
        {
            //dissable ground button
            StartButton.gameObject.SetActive(false);
            hoverObject = null;

            DustyManager.Instance.Messages.Add(new DustyTextFile("Pas goed op de dikke vogels", 5, AudioSampleManager.Instance.DustyRonde02[0]));
            CurrentRound = Round.Round_2;
            GameState = GameStates.Playing;

            SpawnManager.Instance.FirstBirdFat = true;
            currentTimer = 0;
            SendTextMessage("Starting Round 2", 4f, Vector2.zero);
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