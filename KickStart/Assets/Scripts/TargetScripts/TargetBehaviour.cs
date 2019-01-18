using EnumStates;
using UnityEngine;
using VrFox;

public class TargetBehaviour : MonoBehaviour
{

    [Header("Bird Values:")]
    public BirdType TypeOfBird;
    [Range(0, 2)]
    public float Speed = 1;
    public float birdScale;

    public int BigBirdLives;

    public bool MovingBird;

    public GameObject Body;
    public GameObject[] Beek;

    private float BirdSoundCounter;
    private float BirdSoundTimer;// = Random.Range(2f, 6f);

    private bool IsHit;
    private Vector3 endPoint;
    private AudioSource audioSource;
    private BulletEvadeScript evadeScript;

    private Vector3 newEndPoint;
    private Vector3 playerOffset;

    //[Header("Refs:")]
    private GameObject Diaper;
    private MeshRenderer[] logoOnDiaper;

    [Space]
    public GameObject DeathParticle;
    public GameObject SmokeParticles;
    public GameObject BirdHitEffect;
    public GameObject BigBirdHitEffect;

    public BirdPath Path;
    public int goalNode;

    private void Start()
    {
        transform.position += new Vector3(Random.Range(-4, 4), 0, Random.Range(-4, 4));
        
        audioSource = GetComponent<AudioSource>();
        evadeScript = GetComponentInChildren<BulletEvadeScript>();
        evadeScript.SetBird(this);
        evadeScript.gameObject.SetActive(false);

        //plays spawn sound of bird
        BirdSoundTimer = Random.Range(3f, 6f);
        BirdSoundCounter = 0;
        AudioManager.Instance.PlayAudio(AudioSampleManager.Instance.getBirdSpawnSounds(), 1, gameObject);

        //First next node
        goalNode = 1;

        if (!Diaper)
        {
            Diaper = GetComponentInChildren<DiaperBehaviour>().gameObject;
        }

        logoOnDiaper = GetComponentsInChildren<MeshRenderer>();
        for (int i = 1; i < logoOnDiaper.Length; i++)
        {
            logoOnDiaper[i].material = GameManager.Instance.White;
        }
        logoOnDiaper[0].material = GameManager.Instance.Blue;

        //Set initial game values
        GetEndPoint();
        Path = new BirdPath(transform.position, endPoint);

        //Set Values specific for this bird
        switch (TypeOfBird)
        {
            case BirdType.Normal:
                Speed = Random.Range(Speed * 0.7f, Speed / 0.7f);
                if (GameManager.Instance.CurrentRound != Round.Round_1) // first round the bird go in a streigt path
                {
                    //Give the birds a swirving or bouncing effect
                    if (Random.Range(0, 3) != 2)
                    {
                        if (Random.Range(0, 3) == 2)
                        {
                            Path.Bouncing();
                        }
                        else
                        {
                            Path.Swerving();
                        }
                    }
                    else
                    {
                        //Fuck up path of bird?
                    }
                }
                //Give birds speed multipliers
                switch (GameManager.Instance.GetDiffictuly)
                {
                    case Difficulty.Noob:
                        Speed *= 0.8f;
                        break;
                    case Difficulty.Beginner:
                        break;
                    case Difficulty.Normal:
                        Speed *= 1.1f;
                        break;
                    case Difficulty.Hard:
                        Speed *= 1.3f;
                        break;
                    default:
                        Debug.LogError("Code should not be reached!");
                        break;
                }

                break;
            case BirdType.Fat:
                Path.Bouncing(); //fat birds always are bouncing
                break;
            case BirdType.Fast:

                break;
            default:
                Debug.LogError("Should not be reached");
                break;
        }

        //If bird is standing still set in front of player
        if (!MovingBird)
        {
            playerOffset = transform.position + GameManager.Instance.Player.transform.position;
        }

        Diaper.SetActive(IsHit);
        if (!IsHit)
        {
            GameManager.Instance.Indicator.AddIndicator(transform, 0);
        }

        //give bird random scale
        birdScale = Random.Range(birdScale * 0.8f, birdScale / 0.8f);
        transform.localScale = new Vector3(birdScale, birdScale, birdScale);

        //get random colors 
        BirdMaterialPreset _preset = SpawnManager.Instance.GetPreset;
        SetBeekMaterial(_preset.GetBeek);
        SetBodyMaterial(_preset.GetBody);

        //fix model rotation
        transform.Rotate(new Vector3(0, 90, 0));
    }

    private void GetEndPoint()
    {
        if (Random.Range(0, 4) == 1)
        {
            //Get direction to player
            Vector3 _dirToPlayer = GameManager.Instance.CurrentPlayer.transform.position - transform.position;
            _dirToPlayer.Normalize();

            //get Distance to player
            float _disToPlayer = Vector3.Distance(transform.position, GameManager.Instance.CurrentPlayer.transform.position) * 2;

            //get opposite position of player
            endPoint = transform.position + (_disToPlayer * _dirToPlayer);
            endPoint.y = Camera.main.transform.position.y + Random.Range(-0.2f, 0.2f);

            transform.LookAt(endPoint);
            // goalRotation = Quaternion.LookRotation(_dirToPlayer, transform.right);

        }
        else
        {
            Vector3 _dirToPlayer = GameManager.Instance.CurrentPlayer.transform.position - transform.position;

            //Get direction to player


            //Randomize _dirToPlayer
            _dirToPlayer.x *= Random.Range(-5, 5);
            _dirToPlayer.z *= Random.Range(-10, -5);
            _dirToPlayer.Normalize();
            //Debug
            //Debug.Log(_dirToPlayer);

            //Create endpoint in front of player
            endPoint = Camera.main.transform.position + new Vector3(Random.Range(-4, 4), 0, Random.Range(-4, 4));
            endPoint.y = Camera.main.transform.position.y + Random.Range(-0.2f, 0.2f);
            //goalRotation = Quaternion.LookRotation(_dirToPlayer, transform.right);
            transform.LookAt(endPoint);
        }
    }

    private void OnDestroy()
    {
        if (audioSource)
        {
            audioSource.Stop();
        }
        SpawnManager.Instance.CurrentBirdCount--;

        if (!IsHit)
            GameManager.Instance.Player.Missed++;
        
        GameManager.Instance.Indicator.RemoveIndicator(transform);
    }

    /// <summary>
    /// Called when the target is hit
    /// </summary>
    public void Hit(Vector3 _hitPosition)
    {
        if (!Diaper)
        {
            Diaper = GetComponentInChildren<DiaperBehaviour>().gameObject;
        }

        GameManager.Instance.Player.HitCount++;


        if (IsHit)
        {
            return;
        }

        if (GameManager.Instance.CurrentRound == Round.Intro)
        {
            if (!GameManager.Instance.FirstBirdHit)
            {
                GameManager.Instance.FirstBirdHit = true;
                Destroy(gameObject, 4);
            }
            else if (!GameManager.Instance.SecondBirdHit)
            {
                GameManager.Instance.SecondBirdHit = true;
                Destroy(gameObject, 4);
            }
        }
         
        Instantiate(BigBirdHitEffect, _hitPosition, Quaternion.identity);
        AudioManager.Instance.PlayAudio(AudioSampleManager.Instance.getBirdHitSounds(), 1);

        switch (TypeOfBird)
        {
            case BirdType.Normal:
                IsHit = true;
                //SpawnManager.Instance.CurrentBirdCount--;
                Diaper.SetActive(true);
                GameManager.Instance.Player.Score++;
                GameManager.Instance.Indicator.RemoveIndicator(transform);
                GameManager.Instance.Player.ScoreFlash();

                
                //hit effect
                Instantiate(SmokeParticles, transform.position, Quaternion.identity);
                break;

            case BirdType.Fat:
                if (BigBirdLives > 0)
                {
                    //Remove one life
                    BigBirdLives--;
                    //big bird first hit feedback

                }
                else //big bird has a diaper on
                {
                    if (!IsHit)
                    {
                        IsHit = true;
                        //SpawnManager.Instance.CurrentBirdCount--;
                        Diaper.SetActive(true);
                        GameManager.Instance.Player.Score += 3;
                        GameManager.Instance.Indicator.RemoveIndicator(transform); //incicator andere kleur!
                        GameManager.Instance.Player.ScoreFlash();

                        //hit effect
                        Instantiate(SmokeParticles, transform.position, Quaternion.identity); // ander particle effect!
                        AudioManager.Instance.PlayAudio(AudioSampleManager.Instance.getFatBirdExplosionSound(), 1);
                    }
                }
                break;
            case BirdType.Fast:
                IsHit = true;

                Diaper.SetActive(true);
                GameManager.Instance.Player.Score += 10;
                GameManager.Instance.Indicator.RemoveIndicator(transform); //incicator andere kleur!

                AudioManager.Instance.PlayAudio(AudioSampleManager.Instance.GetExplosionSound(), 1);
                //hit effect
                Instantiate(SmokeParticles, transform.position, Quaternion.identity); // ander particle effect!
                break;
            default:
                Debug.LogError("Enum error!");
                break;
        }
    }

    private void SetGoalRotation(Vector3 _goal)
    {
        // Vector3 _dir = _goal - transform.position;
        //float _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;

        //goalRotation = Quaternion.AngleAxis(_angle - 45 + 180, Vector3.up);
    }

    private void Update()
    {
        if (!GameManager.Instance.GameStarted) //Needs rework!
        {
            Destroy(gameObject);
        }

        BirdMovement();

        // plays flying/flapping sound of bird
        BirdFlapSound();
        //Draw path lines
        DrawDebug();
    }

    private void BirdMovement()
    {
        if (MovingBird)
        {
            if (Vector3.Distance(transform.position, Path.Nodes[goalNode].GetPosition) < 0.1f)
            {
                if (goalNode < Path.Nodes.Count - 1)
                {
                    goalNode++;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, Path.Nodes[goalNode].GetPosition, Speed * Time.deltaTime);
                //SetGoalRotation(Path.Nodes[goalNode].GetPosition);
            }

            // transform.rotation = Quaternion.Slerp(transform.rotation, goalRotation, Time.deltaTime * 5);

            //If end point is reached...
            if (Vector3.Distance(transform.position, endPoint) < 1)
            {
                SpawnManager.Instance.CreateParticleEffect(IsHit, transform.position);
                if (!IsHit)
                {
                    //Get minus points when a birds get trough
                    //GameManager.Instance.Player.Score -= 1;
                }
                Destroy(gameObject);

            }
            else if (CheckIfBirdBehindPlayer())
            {
                SpawnManager.Instance.CreateParticleEffect(IsHit, transform.position);
                if (!IsHit)
                {
                    //Get minus points when a birds get trough
                    //GameManager.Instance.Player.Score -= 1;
                }

                Destroy(gameObject);

            }
        }
        else
        {
            //Stay in front of player
            transform.position = GameManager.Instance.Player.transform.position + (playerOffset / 1.5f);
        }
    }

    private bool CheckIfBirdBehindPlayer()
    {
        return false;
        if (transform.position.z < (Camera.main.transform.position.z - 0.5))
        {
            return true;
        }
        if (transform.position.x < (Camera.main.transform.position.x) - 3.8)
        {
            if (transform.position.z < Camera.main.transform.position.z - 0.5)
            {
                return true;
            }
        }
        if (transform.position.x > (Camera.main.transform.position.x) + 3.8)
        {
            if (transform.position.z < (Camera.main.transform.position.z) - 0.5)
            {
                return true;
            }
        }
        return false;
    }

    private void BirdFlapSound()
    {
        BirdSoundCounter += Time.deltaTime;
        if (BirdSoundCounter > BirdSoundTimer)
        {
            BirdSoundCounter = 0;
            AudioManager.Instance.PlayAudio(AudioSampleManager.Instance.getBirdFlapSounds(), 1, gameObject);
        }

        BirdSoundTimer = Random.Range(3f, 6f);
    }

    /// <summary>
    /// Set the beek of the bird to a certain metrial
    /// </summary>
    /// <param material of the bird="_mat"></param>
    private void SetBeekMaterial(Material _mat)
    {
        return;
        foreach (var item in Beek)
        {
            item.GetComponent<Renderer>().material = _mat;
        }
    }

    /// <summary>
    /// Set the body of the bird to a certain material
    /// </summary>
    /// <param given material="_mat"></param>
    private void SetBodyMaterial(Material _mat)
    {
        return;
        Body.GetComponent<Renderer>().material = _mat;
    }

    private void DrawDebug()
    {
        if (MovingBird)
        {
            for (int i = 0; i < Path.Nodes.Count; i++)
            {
                if (i < Path.Nodes.Count - 1)
                {
                    Debug.DrawLine(Path.Nodes[i].GetPosition, Path.Nodes[i + 1].GetPosition, Color.cyan);
                }
            }
        }
    }

}
