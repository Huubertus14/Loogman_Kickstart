using EnumStates;
using UnityEngine;
using UnityEngine.UI;

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
        public UIWiggle WaterUIWiggle;
        public GameObject HighScoreObject;
        public Text AccuracyText;

        [Header("Values:")]
        public string PlayerName;
        public int Score;
        public float TimeToRecharge;
        public Difficulty PlayerLevel;
        public bool InPrewash;

        public int ShootCount;
        public int HitCount;

        [Space]
        public bool IsSynced;
        private Vector3[] lastPositions = new Vector3[5];
        private int posIndex;

        public void ResetPlayerValues()
        {
            Score = 0;
            PlayerName = "Loogman Devop";
            PlayerLevel = Difficulty.Noob;
            InPrewash = true;

            AccuracyText.text = "";

            ShootCount = 0;
            HitCount = 0;

            IsSynced = false;

            for (int i = 0; i < lastPositions.Length; i++)
            {
                lastPositions[i] = Vector3.zero;
            }
            posIndex = 0;
        }

        //Player Shoots a bullet
        public void Shoot()
        {
            AudioManager.Instance.PlayAudio(AudioSampleManager.Instance.GetShootSound(), 1);
            GameObject _bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
            _bullet.GetComponent<BulletBehaviour>().ShootBullet();
            ShootCount++;

            if (Score < 10)
            {
                if (PlayerLevel != Difficulty.Noob)
                {
                    SpawnManager.Instance.MaxBirdCount = 2;
                }
                PlayerLevel = Difficulty.Noob;
            }
            else if (Score < 15)
            {
                if (PlayerLevel != Difficulty.Beginner)
                {
                    SpawnManager.Instance.MaxBirdCount = 5;
                }
                PlayerLevel = Difficulty.Beginner;
            }
            else if (Score < 25)
            {
                if (PlayerLevel != Difficulty.Normal)
                {
                    SpawnManager.Instance.MaxBirdCount = 8;
                }
                PlayerLevel = Difficulty.Normal;
            }
            else
            {
                if (PlayerLevel != Difficulty.Hard)
                {
                    SpawnManager.Instance.MaxBirdCount = 12;
                }
                PlayerLevel = Difficulty.Hard;
            }
        }

        private void Update()
        {
            PlayerDebug();
            SetAccuracyText();
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

        private void SetAccuracyText()
        {
            if (ShootCount > 0)
            {
                float _accuracy = (HitCount / ShootCount) * 100;
                AccuracyText.text = "";
            }
        }

        private void PlayerDebug()
        {
            //Draw line in look direction
            Debug.DrawLine(transform.position, Camera.main.transform.forward * 15, Color.red);
        }
    }
}