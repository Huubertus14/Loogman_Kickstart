using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFlashing : MonoBehaviour {

    [Header("Values:")]
    public float Duration;
    public AnimationCurve curve;


    private float timeTweenKey, tweenValue;

    private Vector3 orginScale;

    private RectTransform rect;
    private Text textObject;
    private Color orginColor = Color.white;
    public Color FlashColor;

	// Use this for initialization
	void Start () {
        textObject = GetComponent<Text>();
        rect = GetComponent<RectTransform>();
        timeTweenKey = 2;
        orginScale = rect.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        if (timeTweenKey < 1)
        {
            timeTweenKey += Time.deltaTime / Duration;

            tweenValue = curve.Evaluate(timeTweenKey);
            Debug.Log(tweenValue);

            rect.localScale = orginScale * tweenValue;

            
        }
        else
        {
            rect.localScale = orginScale;
            textObject.color = Color.Lerp(textObject.color, orginColor, Time.deltaTime  * 5);
            
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartEffect();
        }
	}


    public void StartEffect()
    {
        timeTweenKey = 0;
        textObject.color = FlashColor;
    }

}
