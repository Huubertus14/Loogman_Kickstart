using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VrFox;
using EnumStates;

namespace VrFox
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [Header("InCarWash")]
        public bool InCarWash;
        [Header("Prefabs:")]
        public GameObject BulletPrefab;

        [Header("Refs:")]
        public TextFlashing ScoreTextFlash;
        public TextFlashing GarbageTextFlash;
        public SliderLerp WaterAmounfSlider;
        public UIWiggle WaterUIWiggle;
        public GameObject HighScoreObject;

        [Header("Values:")]
        public string PlayerName;
        public int Score;
        public int HitByGarbage;
        public float AmountOfWater;
        public float MaxAmountOfWater;
        public float TimeToRecharge;
        public Difficulty PlayerLevel;
        public bool InPrewash;

        [Space]
        public bool IsSynced;
        private Vector3[] lastPositions = new Vector3[5];
        private int posIndex;

        public void ResetPlayerValues()
        {
            AmountOfWater = MaxAmountOfWater;
            WaterAmounfSlider.SetMaxValue(MaxAmountOfWater);
            WaterAmounfSlider.SetGoalValue(AmountOfWater);

            Score = 0;
            PlayerName = "Loogman Devop";
            HitByGarbage = 0;
            PlayerLevel = Difficulty.Noob;
            InPrewash = true;

            IsSynced = false;

            for (int i = 0; i < lastPositions.Length; i++)
            {
                lastPositions[i] = Vector3.zero;
            }
            posIndex = 0;

           // SyncCarWashWithPlayer(0);
        }

        //Player Shoots a bullet
        public void Shoot()
        {
            if (AmountOfWater > 1)
            {
                AudioManager.Instance.PlayAudio(AudioSampleManager.Instance.GetShootSound(), 1);
                GameObject _bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
                _bullet.GetComponent<BulletBehaviour>().ShootBullet();

                AmountOfWater--;
                WaterAmounfSlider.SetGoalValue(AmountOfWater);
            }
            else
            {
                WaterUIWiggle.StartAnimation();
            }

            if (Score < 10)
            {
                if (PlayerLevel != Difficulty.Noob)
                {
                    DustyManager.Instance.Messages.Add(new DustyTextFile("Try your best!", 2f, 1f));
                    SpawnManager.Instance.MaxBirdCount = 2;
                }
                PlayerLevel = Difficulty.Noob;
            }
            else if (Score < 15)
            {
                if (PlayerLevel != Difficulty.Beginner)
                {
                    DustyManager.Instance.Messages.Add(new DustyTextFile("More are coming!", 2f, 1f));
                    SpawnManager.Instance.MaxBirdCount = 5;
                }
                PlayerLevel = Difficulty.Beginner;
            }
            else if (Score < 25)
            {
                if (PlayerLevel != Difficulty.Normal)
                {
                    DustyManager.Instance.Messages.Add(new DustyTextFile("Let's make it a bit more difficult!", 2f, 1f));
                    SpawnManager.Instance.MaxBirdCount = 8;
                }
                PlayerLevel = Difficulty.Normal;
            }
            else
            {
                if (PlayerLevel != Difficulty.Hard)
                {
                    DustyManager.Instance.Messages.Add(new DustyTextFile("There is no going back now! Keep Shooting!", 2f, 1f));
                    SpawnManager.Instance.MaxBirdCount = 12;
                }
                PlayerLevel = Difficulty.Hard;
            }
        }

        private void Update()
        {
            //Increase Water Value
            if (!GameManager.Instance.GameStarted)
            {
                return;
            }

            if (!InCarWash)
            {
                //+ (Time.deltaTime / 46) * 110)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (Time.deltaTime / 46) * 75);
            }

            AmountOfWater += Time.deltaTime / TimeToRecharge;
            WaterAmounfSlider.SetGoalValue(AmountOfWater);

            if (!IsSynced)
            {
                //fill last positions
                lastPositions[posIndex] = transform.position;
                posIndex++;
                if (posIndex >= lastPositions.Length)
                {
                    posIndex = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                SyncCarWashWithPlayer(1);
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                Shoot();
            }
        }

        public void ScoreFlash()
        {
            ScoreTextFlash.StartEffect();
            if (Score > 9 && Score < 11)
            {
                //GameManager.Instance.SendTextMessage("Some may spawn with a diaper", 3, Vector2.zero);
            }

            if (Score < 10)
            {
                PlayerLevel = Difficulty.Noob;
            }
            else if (Score < 15)
            {
                PlayerLevel = Difficulty.Beginner;
            }
            else if (Score < 25)
            {
                PlayerLevel = Difficulty.Normal;
            }
            else
            {
                PlayerLevel = Difficulty.Hard;
            }
        }

        public void SyncCarWashWithPlayer(int _checkpointCount)
        {
            IsSynced = true;

            //Fix the right rotation
            //int _currentIndex = posIndex;
            //int _lastIndex = posIndex + 1;
            //if (_lastIndex > 4)
            //{
            //    _lastIndex = 0;
            //}

            //  Vector3 _direction = lastPositions[_lastIndex] - lastPositions[_currentIndex];
            Vector3 _direction = Camera.main.transform.forward;

            //Normalize the values
            _direction.Normalize();

            //Set vertical values to 0
            _direction.y = 0;

            //Calculate the Y angle
            float _angle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;

            //Set new rotation using angle
            Quaternion _newWorldRot = Quaternion.AngleAxis(_angle, CarWashWorld.Instance.transform.up);

            //Rotate the carwash
           // CarWashWorld.Instance.transform.rotation = _newWorldRot;
            CarWashWorld.Instance.SetGoalRotation(_newWorldRot);

            //Place player in the right position
            //Chaeck later for eventual more becons
            
            if (_checkpointCount >= CarWashWorld.Instance.Checkpoint.Length)
            {
                //CarWashWorld.Instance.transform.position = transform.position - CarWashWorld.Instance.Checkpoint[0].transform.localPosition;
                CarWashWorld.Instance.SetGoalPosition(transform.position - CarWashWorld.Instance.Checkpoint[0].transform.localPosition);
                Debug.LogError("Unable to find this checkpoint - " + "Index: " + _checkpointCount + "\nMax Index = " + (CarWashWorld.Instance.Checkpoint.Length - 1));
            }
            else
            {
                //CarWashWorld.Instance.transform.position = transform.position - CarWashWorld.Instance.Checkpoint[_checkpointCount].transform.localPosition;
                CarWashWorld.Instance.SetGoalPosition(transform.position - CarWashWorld.Instance.Checkpoint[_checkpointCount].transform.localPosition);
            }
        }
    }
}