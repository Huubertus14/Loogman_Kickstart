using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public static SpawnManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("Prefabs")]
    public GameObject BirdPrefab;

    [Header("Values")]
    public float SpawnInterval;
    private float spawnTimer;
    public int MaxBirdCount;
    public int CurrentBirdCount;

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
            SpawnBird();
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
            _bird.transform.position = new Vector3(_bird.transform.position.x, Random.Range(0.1f, 1.8f), _bird.transform.position.z);

            lastBird = _bird;
        }
    }

    public GameObject GetLastBird
    {
        get { return lastBird; }
    }
}
