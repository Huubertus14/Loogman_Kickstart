using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VrFox;

namespace VrFox
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [Header("Prefabs:")]
        public GameObject BulletPrefab;


        [Header("Refs:")]
        public TextFlashing ScoreTextFlash;
        public TextFlashing GarbageTextFlash;
        public SliderLerp WaterAmounfSlider;
        public UIWiggle WaterUIWiggle;

        [Header("Values:")]
        public string PlayerName;
        public int Score;
        public int HitByGarbage;
        public float AmountOfWater;
        public float MaxAmountOfWater;
        public float TimeToRecharge;

        public void ResetPlayerValues()
        {
            AmountOfWater = MaxAmountOfWater;
            WaterAmounfSlider.SetMaxValue(MaxAmountOfWater);
            WaterAmounfSlider.SetGoalValue(AmountOfWater);
            Score = 0;
            PlayerName = "Loogman Devop";
            HitByGarbage = 0;
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
        }

        private void Update()
        {
            //Increase Water Value
            if (!GameManager.Instance.GameStarted)
            {
                return;
            }

            AmountOfWater += Time.deltaTime / TimeToRecharge;
            WaterAmounfSlider.SetGoalValue(AmountOfWater);
        }

        public void ScoreFlash()
        {
            ScoreTextFlash.StartEffect();
            if (Score > 9 && Score < 11)
            {
                //GameManager.Instance.SendTextMessage("Some may spawn with a diaper", 3, Vector2.zero);
            }
        }

        public void GarbageFlash()
        {
            GarbageTextFlash.StartEffect();
        }

    }
}