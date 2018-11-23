using UnityEngine;
using VrFox;
using EnumStates;

public class TargetBehaviour : MonoBehaviour
{

    [Header("Bird Values:")]
    [Range(1, 15)]
    public float Speed = 1;
    public float BirdSoundCounter;
    public float BirdSoundTimer;// = Random.Range(2f, 6f);
    public bool MovingBird;

    public GameObject Body;

    public GameObject[] Beek;

    [Space]
    private bool IsHit;
    private Vector3 endPoint;
    private AudioSource audioSource;

    private Vector3 newEndPoint;
    private Vector3 playerOffset;

    //[Header("Refs:")]
    private GameObject Diaper;
    private MeshRenderer[] logoOnDiaper;
    private bool alwaysDiaperOn;

    private float birdScale;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //plays spawn sound of bird
        BirdSoundTimer = Random.Range(3f, 6f);
        BirdSoundCounter = 0;
        AudioManager.Instance.PlayAudio(AudioSampleManager.Instance.getBirdSpawnSounds(), 1, gameObject);

        if (!Diaper)
        {
            Diaper = GetComponentInChildren<DiaperBehaviour>().gameObject;
        }

        logoOnDiaper = GetComponentsInChildren<MeshRenderer>();
        for (int i = 1; i < logoOnDiaper.Length; i++)
        {
            logoOnDiaper[i].material = GameManager.Instance.Blue;
        }
        logoOnDiaper[0].material = GameManager.Instance.White;

        Speed = Random.Range(0.01f, 0.015f);

        switch (GameManager.Instance.GetDiffictuly)
        {
            case Difficulty.Noob:
                Speed *= 0.8f;
                break;
            case Difficulty.Beginner:
                break;
            case Difficulty.Normal:
                Speed *= 1.1f;
                IsHit = ((int)Random.Range(0, 8) == 2);
                break;
            case Difficulty.Hard:
                Speed *= 1.3f;
                IsHit = ((int)Random.Range(0, 6) == 2);
                break;
            default:
                break;
        }

        if (!MovingBird)
        {
            playerOffset = transform.position + GameManager.Instance.Player.transform.position;
        }

        Diaper.SetActive(IsHit);
        if (!IsHit)
        {
            GameManager.Instance.Indicator.AddIndicator(transform, 0);
        }

        GetEndPoint();

        //give bird random scale
        birdScale = Random.Range(0.9f, 1.1f);
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
        if ((int)Random.Range(0, 4) == 1)
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
        }
        else
        {
            //Get direction to player
            Vector3 _dirToPlayer = GameManager.Instance.CurrentPlayer.transform.position - transform.position;

            //Randomize _dirToPlayer
            _dirToPlayer.x = Random.Range(-5, 5);
            _dirToPlayer.z = Random.Range(-10, -5);
            _dirToPlayer.Normalize();
            //Debug
            //Debug.Log(_dirToPlayer);

            //Create endpoint in front of player
            endPoint = transform.position + (Random.Range(5, 10) * _dirToPlayer);
            endPoint.y = Camera.main.transform.position.y + Random.Range(-0.2f, 0.2f);

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
        GameManager.Instance.Indicator.RemoveIndicator(transform);
    }

    /// <summary>
    /// Called when the target is hit
    /// </summary>
    public void Hit()
    {
        if (!Diaper)
        {
            Diaper = GetComponentInChildren<DiaperBehaviour>().gameObject;
        }

        if (alwaysDiaperOn)
        {
            return;
        }

        IsHit = !IsHit;

        if (IsHit)
        {
            Diaper.SetActive(true);
            GameManager.Instance.Player.Score++;
            GameManager.Instance.Indicator.RemoveIndicator(transform);
        }
        else
        {
            Diaper.SetActive(false);
            GameManager.Instance.Player.Score -= 2;
            GameManager.Instance.SendTextMessage("Schiet niet op de vogels die al een luier om hebben!", 2.5f, Vector2.zero);
            GameManager.Instance.Indicator.AddIndicator(transform, 0);
        }

        //Do something different when in a tutorial
        if (SpawnManager.Instance.TutorialActive)
        {
            Debug.Log("Tut bord hit");
            DustyManager.Instance.Messages.Add(new DustyTextFile("Pats!", 6, AudioSampleManager.Instance.DustyText[11]));

            SpawnManager.Instance.TutorialBirdsShot++;

            Destroy(gameObject, 4);
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.GameStarted)
        {
            Destroy(gameObject);
        }
        if (MovingBird)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoint, Speed);

            //If end point is reached...
            if (Vector3.Distance(transform.position, endPoint) < 1)
            {
                SpawnManager.Instance.CreateParticleEffect(IsHit, transform.position);
                Destroy(gameObject);

            }
            else if (CheckIfBirdBehindPlayer())
            {
                SpawnManager.Instance.CreateParticleEffect(IsHit, transform.position);
                Destroy(gameObject);
            }
        }
        else
        {
            //Stay in fornt of playertr
            transform.position = GameManager.Instance.Player.transform.position + (playerOffset / 1.5f);
        }

        // plays flying/flapping sound of bird
        BirdFlapSound();

    }

    private bool CheckIfBirdBehindPlayer()
    {
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

    public void SetAlwaysDiaperOn()
    {
        if (!Diaper)
        {
            Diaper = GetComponentInChildren<DiaperBehaviour>().gameObject;
        }

        Diaper.SetActive(true);
        GameManager.Instance.Player.Score++;
        GameManager.Instance.Indicator.RemoveIndicator(transform);
        alwaysDiaperOn = true;
    }

    /// <summary>
    /// Set the beek of the bird to a certain metrial
    /// </summary>
    /// <param material of the bird="_mat"></param>
    private void SetBeekMaterial(Material _mat)
    {
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
        Body.GetComponent<Renderer>().material = _mat;
    }

}
