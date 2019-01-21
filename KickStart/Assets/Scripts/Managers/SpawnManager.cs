using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFox;
using EnumStates;

namespace VrFox
{
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Instance;
        private void Awake()
        {
            Instance = this;
        }

        [Header("Prefabs")]
        public GameObject BirdPrefab;
        public GameObject StaticBirdPrefab;
        public GameObject FatBirdPrefab;
        public GameObject SpeedBird;

        [Header("Refs:")]
        public List<GameObject> Spawns = new List<GameObject>();

        [Header("Values")]
        public float SpawnInterval;
        private float spawnTimer;
        public int MaxBirdCount;
        public int CurrentBirdCount;
        [Space]
        public GameObject DeathParticle;
        public GameObject SmokeParticles;
        public GameObject BirdHitEffect;

        public GameObject OrientationHolder;

        [Space]
        public BirdMaterialPreset[] BirdMaterials;

        public List<GameObject> Birds = new List<GameObject>();

        public bool FirstBirdFat;

        private void Update()
        {
            if (!GameManager.Instance.GameStarted)
            {
                return;
            }
            if (GameManager.Instance.GameState == GameStates.Playing)
            {
                GamePlay();
            }
        }
        
        private void GamePlay()
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > SpawnInterval)
            {
                if (CurrentBirdCount <= MaxBirdCount)
                {
                    if (GameManager.Instance.CurrentRound != Round.Round_1)
                    {
                        if (FirstBirdFat)
                        {
                            FirstBirdFat = false;
                            SpawnBird(FatBirdPrefab);//spawn fat bird
                            spawnTimer = 0;
                            return;
                        }
                        if (Random.Range(0, 5) == 2)//dice roll which bird is to spawn
                        {
                            SpawnBird(FatBirdPrefab);//spawn fat bird
                            spawnTimer = 0;
                        }
                        else
                        {
                            SpawnBird(BirdPrefab);//spawn normal bird
                            spawnTimer = 0;
                        }
                    }
                    else //if round 1 spawn normal birds
                    {
                        SpawnBird(BirdPrefab);//spawn normal bird
                        spawnTimer = 0;
                    }
                }
            }

            if (GameManager.Instance.CurrentRound == Round.Round_3)
            {
                //Do the fast bird loop
            }
        }
        
        /// <summary>
        /// Called to check the round and difficulty of the game
        /// </summary>
        private void DetermineDifficulty()
        {
            switch (GameManager.Instance.CurrentRound)
            {
                case Round.Intro:
                    //No difficutly
                    break;
                case Round.Round_1:
                    SpawnInterval = Random.Range(3.5f, 5.5f);
                    MaxBirdCount = 5;
                    break;
                case Round.Round_2:
                    SpawnInterval = Random.Range(1.5f, 2.5f);
                    MaxBirdCount = 9;
                    break;
                case Round.Round_3:
                    SpawnInterval = Random.Range(0.6f, 1f);
                    MaxBirdCount = 21;
                    break;
                case Round.Score:
                    //No difficutly
                    break;
                default:
                    //No difficutly
                    break;
            }
        }

        public void SpawnBird(GameObject _prefab)
        {
            CurrentBirdCount++;

            //Spawn bird
            GameObject _bird = Instantiate(_prefab, transform.position, Quaternion.identity);
            Birds.Add(_bird);

            //Get the y value of the bird
            float _spawnY = Camera.main.transform.position.y + Random.Range(-0.2f, 0.2f);

            //Set right spawn point 
            //Vector3 _direction = new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-1, 1), 1);

            Vector3 _direction = Camera.main.transform.forward;

            float _distance = Random.Range(7, 12);
            
            Vector3 _spawnPos = _direction * _distance;
           // _spawnPos.x *= Random.Range(-0.5f,0.5f);
           // _spawnPos.z *= Random.Range(-0.5f,0.5f);
            _spawnPos.y = _spawnY;

            _bird.transform.position = _spawnPos;

            DetermineDifficulty();
        }

        public void SpawnBirdInFrontOfPlayer()
        {
            CurrentBirdCount++;

            //Spawn bird
            GameObject _bird = Instantiate(StaticBirdPrefab, transform.position, Quaternion.identity);

            //Set right spawn point 
            Vector3 _direction = new Vector3(0,0,1);
            float _distance = Random.Range(4, 6);

            _bird.transform.position = _direction * _distance;
            _bird.transform.position = new Vector3(_bird.transform.position.x, Random.Range(-0.5f, 1.2f), _bird.transform.position.z);
            
        }

        public void ClearAllBirds()
        {
            foreach (var _bird in Birds)
            {
                if (_bird)
                {
                    Destroy(_bird);
                }
                else
                {
                    Debug.Log("Bird allready gone");
                }
            }
            Birds.Clear();
            Debug.Log("All birds gone");
        }

        public void CreateParticleEffect(bool _IsHit, Vector3 _birdPosition)
        {
            //Check if killed or missed!
            //Depending on type of death add effect

           // CurrentBirdCount--;
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

        public BirdMaterialPreset GetPreset
        {
            get
            {
                int _x = Random.Range(0, BirdMaterials.Length);
                return BirdMaterials[_x];
            }
        }
    }
}