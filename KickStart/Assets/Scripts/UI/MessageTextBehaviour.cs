using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MessageTextBehaviour : MonoBehaviour {

    private float duration;
    public AnimationCurve ScaleCurve;
    private float tweenValue, timeTweenKey;

    private Text messageText;
    private RectTransform rect;

	// Use this for initialization
	void Start () {
        timeTweenKey = 2;

        messageText = GetComponent<Text>();
        rect = GetComponent<RectTransform>();
        rect.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        if (timeTweenKey < 1)
        {
            timeTweenKey += Time.deltaTime / duration;
            tweenValue = ScaleCurve.Evaluate(timeTweenKey);
            rect.localScale = new Vector3(tweenValue,tweenValue,tweenValue);
        }
	}


    public void Message(string _message, float _duration, Vector2 _offset)
    {
        duration = _duration;
        messageText.text = _message;
        rect.transform.localPosition = new Vector3(_offset.x, _offset.y, 0);
        timeTweenKey = 0;
    }
}
