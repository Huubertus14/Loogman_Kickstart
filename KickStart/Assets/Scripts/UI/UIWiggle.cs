using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWiggle : MonoBehaviour {

    RectTransform rect;

    public AnimationCurve curve;
    public float duration;
    private float tweenValue, timeTweenKey;
    Vector3 orginScale;

	// Use this for initialization
	void Start () {
        rect = GetComponent<RectTransform>();
        timeTweenKey = 2;
        orginScale = rect.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        if (timeTweenKey < 1)
        {
            timeTweenKey += Time.deltaTime / duration;
            tweenValue = curve.Evaluate(timeTweenKey);
        }
        else
        {
            tweenValue = 1;
        }
        rect.localScale = orginScale * tweenValue;
    }


    public void StartAnimation()
    {
        timeTweenKey = 0;
    }
}
