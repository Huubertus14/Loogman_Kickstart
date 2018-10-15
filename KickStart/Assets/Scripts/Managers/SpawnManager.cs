using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFox;

public class SpawnManager : MonoBehaviour {

    public static SpawnManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("Prefabs")]
    public GameObject BirdPrefab;
    public GameObject StaticBirdPrefab;
    
    [Header("Values")]
    public float SpawnInterval;
    private float spawnTimer;
    public int MaxBirdCount;
    public int CurrentBirdCount;
    [Space]
    public GameObject DeathParticle;
    public GameObject SmokeParticles;
    public GameObject BirdHitEffect;

    private GameObject lastBird;

    private void Update()
    {
        if (!GameManager.Instance.GameStarted)
        {
            return;
        }
        spawnTimer += Time.deltaTime;
        if (spawnTimer > SpawnInterval)
        {
            spawnTimer = 0;
            if (GameManager.Instance.Player.Score > 3)
            {
                //spawn normal bird
                SpawnBird();
            }
            else
            {
                //spawn Static bird
                SpawnBirdInFrontOfPlayer();
            }
        }
    }


    public void SpawnBird()
    {
        if (CurrentBirdCount <= MaxBirdCount)
        {
            CurrentBirdCount++;

            //Spawn bird
            GameObject _bird = Instantiate(BirdPrefab, transform.position, Quaternion.identity);
            GameManager.Instance.Targets.Add(_bird.GetComponentInChildren<Renderer>());

            float _spawnY = Camera.main.transform.position.y + Random.Range(-0.2f,0.2f);

            //Set right spawn point 
            Vector3 _direction =  new Vector3(Random.Range(-1,1), Random.Range(-1, 1), Random.Range(-1, 1));
            float _distance = Random.Range(-22, 22);
           
            // Debug.Log(_direction * _distance);
            if (_distance < 5)
            {
                _distance = 5;
            }
            else if (_distance > -5)
            {
                _distance = -5;
            }

            _bird.transform.position = _direction * _distance;
            _bird.transform.position = new Vector3(_bird.transform.position.x, _spawnY, _bird.transform.position.z);

            lastBird = _bird;
            
        }
    }

    public void SpawnBirdInFrontOfPlayer()
    {
        if (CurrentBirdCount > 0)
        {
            return;
        }
        CurrentBirdCount++;

        //Spawn bird
        GameObject _bird = Instantiate(StaticBirdPrefab, transform.position, Quaternion.identity);
        GameManager.Instance.Targets.Add(_bird.GetComponentInChildren<Renderer>());

        //Set right spawn point 
        Vector3 _direction = Camera.main.transform.forward;
        float _distance = Random.Range(2, 5);
        
        _bird.transform.position = _direction * _distance;
        _bird.transform.position = new Vector3(_bird.transform.position.x, Random.Range(-0.5f, 1.2f), _bird.transform.position.z);

        lastBird = _bird;
    }

    public void CreateParticleEffect(bool _IsHit, Vector3 _birdPosition)
    {
        //Check if killed or missed!
        //Depending on type of death add effect

        CurrentBirdCount--;
        if (_IsHit)
        {
            if (DeathParticle)
            {
                Instantiate(DeathParticle, _birdPosition, Quaternion.identity);
            }
        }
        else
        {
            if (SmokeParticles)
            {
                Instantiate(SmokeParticles, _birdPosition, Quaternion.identity);
            }
        }
    }

    public void BirdHit(Vector3 _position)
    {
        Instantiate(BirdHitEffect, _position, Quaternion.identity);
    }

    public GameObject GetLastBird
    {
        get { return lastBird; }
    }
     
}
