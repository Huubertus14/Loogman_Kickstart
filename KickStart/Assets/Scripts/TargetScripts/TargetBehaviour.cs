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
    [Space]
    public bool IsHit;
    public GameObject GarbagePrefab;
    private Vector3 endPoint;

    

    private void Start()
    {
        GameManager.Instance.Indicator.AddIndicator(transform, 0);
        Speed = Random.Range(0.01f, 0.015f);
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
        SpawnManager.Instance.CreateParticleEffect(IsHit);
    }
    

    private void Update()
    {

        if (!GameManager.Instance.GameStarted)
        {
            Destroy(gameObject);
        }
        transform.position = Vector3.MoveTowards(transform.position, endPoint, Speed);

        //Id end point is reached...
        if (Vector3.Distance(transform.position, endPoint) < 1)
        {
            Destroy(gameObject);
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
