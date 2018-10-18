using UnityEngine;
using VrFox;

public class TargetBehaviour : MonoBehaviour
{

    [Header("Bird Values:")]
    [Range(1, 15)]
    public float Speed = 1;
    public float BirdSoundCounter;
    public float BirdSoundTimer;// = Random.Range(2f, 6f);
    public bool MovingBird;
    [Space]
    [HideInInspector]
    private bool IsHit;
    public GameObject GarbagePrefab;
    private Vector3 endPoint;
    private AudioSource audioSource;

    //[Header("Refs:")]
    private GameObject Diaper;

    public bool OnScreen;
    Vector3 screenPoint;// = playerHead.leftCamera.WorldToViewportPoint(targetPoint.position);

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        //plays spawn sound of bird
        BirdSoundTimer = Random.Range(3f, 6f);
        BirdSoundCounter = 0;
        AudioManager.Instance.PlayAudio(AudioSampleManager.Instance.getBirdSpawnSounds(), 1, gameObject);

        GameManager.Instance.Indicator.AddIndicator(transform, 0);
        Speed = Random.Range(0.01f, 0.015f);
        if (!Diaper)
        {
            Diaper = GetComponentInChildren<DiaperBehaviour>().gameObject;
        }
        if (GameManager.Instance.Player.Score > 9)
        {
            IsHit = (Random.Range(1, 10) % 2 == 0);
        }
        else
        {
            IsHit = false;
        }

        Diaper.SetActive(IsHit);
        GetEndPoint();
    }

    private void GetEndPoint()
    {
        //Get direction to player
        Vector3 _dirToPlayer = GameManager.Instance.CurrentPlayer.transform.position - transform.position;
        _dirToPlayer.Normalize();

        //get Distance to player
        float _disToPlayer = Vector3.Distance(transform.position, GameManager.Instance.CurrentPlayer.transform.position) * 2;

        //get opposite position of player
        endPoint = transform.position + (_dirToPlayer * _disToPlayer);
        endPoint.y = Camera.main.transform.position.y + Random.Range(-0.2f, 0.2f);

        transform.LookAt(endPoint);
    }

    private void OnDestroy()
    {
        audioSource.Stop();
        SpawnManager.Instance.CurrentBirdCount--;
        GameManager.Instance.Indicator.RemoveIndicator(transform);
    }

    public void Hit()
    {
        if (!Diaper)
        {
            Diaper = GetComponentInChildren<DiaperBehaviour>().gameObject;
        }

        IsHit = !IsHit;

        if (IsHit)
        {
            Diaper.SetActive(true);
            GameManager.Instance.Player.Score++;
        }
        else
        {
            Diaper.SetActive(false);
            GameManager.Instance.Player.Score -= 2;
            GameManager.Instance.SendTextMessage("Don't Shoot those whom already have a diaper on!", 2.5f, Vector2.zero);
        }

        if (GameManager.Instance.Player.Score < 5)
        {
            //Check if first thing
            //gameObject.AddComponent<CleanUp>();
            GetComponent<CleanUp>().LifeTime = 3;
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

            //Id end point is reached...
            if (Vector3.Distance(transform.position, endPoint) < 1)
            {
                SpawnManager.Instance.CreateParticleEffect(IsHit, transform.position);
                Destroy(gameObject);
            }
        }

        // plays flying/flapping sound of bird
        BirdFlapSound();
       
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

    public void ThrowGarbage()
    {
        // Instantiate(GarbagePrefab,transform.position, Quaternion.identity);
        // Debug.Log("Trow Garbage");

    }
}
