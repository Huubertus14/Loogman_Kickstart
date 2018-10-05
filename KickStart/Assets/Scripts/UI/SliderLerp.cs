using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderLerp : MonoBehaviour {

    private Slider slider;
    private float goalValue;
    public float MaxValue;
    public float Speed;

	// Use this for initialization
	void Start () {
        slider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        slider.value = Mathf.Lerp(slider.value, goalValue, Time.deltaTime * Speed);
	}

    public void SetGoalValue(float _value)
    {
        goalValue = _value;
    }

    public void SetMaxValue(float _value)
    {
        MaxValue = _value;
        slider.maxValue = MaxValue;
    }

    public void SetSpeed(float _speed)
    {
        Speed = _speed;
    }
}
