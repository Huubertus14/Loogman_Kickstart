﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour {

    [Header("Prefabs:")]
    public GameObject BulletPrefab;


    [Header("Refs:")]
    public TextFlashing ScoreTextFlash;
    public TextFlashing GarbageTextFlash;
    public SliderLerp WaterAmounfSlider;
    public UIWiggle WaterUIWiggle;

    [Header("Values:")]
    public float AmountOfWater;
    public float MaxAmountOfWater;
    public float TimeToRecharge;
    
    public void ResetPlayerValues()
    {
        AmountOfWater = MaxAmountOfWater;
        WaterAmounfSlider.SetMaxValue(MaxAmountOfWater);
        WaterAmounfSlider.SetGoalValue(AmountOfWater);
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

        AmountOfWater += Time.deltaTime/ TimeToRecharge;
        WaterAmounfSlider.SetGoalValue(AmountOfWater);
    }

    public void ScoreFlash()
    {
        ScoreTextFlash.StartEffect();
    }

    public void GarbageFlash()
    {
        GarbageTextFlash.StartEffect();
    }

}
