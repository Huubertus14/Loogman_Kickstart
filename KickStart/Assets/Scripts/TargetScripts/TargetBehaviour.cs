using System.Collections;
using System.Collections.Generic;
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

    [Header("Refs:")]
    public GameObject Diaper;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GameManager.Instance.Indicator.AddIndicator(transform, 0);
        Speed = Random.Range(0.01f, 0.015f);

        Diaper.SetActive(false);

        GetEndPoint();
    }

    private void GetEndPoint()
    {
        //get end point

        //Get direction to player
        Vector3 _dirToPlayer = GameManager.Instance.CurrentPlayer.transform.position - transform.position;
        _dirToPlayer.Normalize();

        //get Distance to player
        float _disToPlayer = Vector3.Distance(transform.position, GameManager.Instance.CurrentPlayer.transform.position) * 2;

        //get opposite position of player
        endPoint = transform.position + (_dirToPlayer * _disToPlayer);
        endPoint.y = Random.Range(0.1f, 1.8f);

        transform.LookAt(endPoint);
    }

    private void OnDestroy()
    {
        audioSource.Stop();
        GameManager.Instance.Indicator.RemoveIndicator(transform);
    }

    public void Hit()
    {
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
        BirdSoundCounter += Time.deltaTime;
        if (BirdSoundCounter > BirdSoundTimer)
        {
            BirdSoundCounter = 0;
            AudioManager.Instance.PlayAudio(AudioSampleManager.Instance.getBirdSounds(), 1, gameObject);
        }

        BirdSoundTimer = Random.Range(3f, 6f);
    }

    public void ThrowGarbage()
    {
        // Instantiate(GarbagePrefab,transform.position, Quaternion.identity);
        // Debug.Log("Trow Garbage");

    }
}
