using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationLerp : MonoBehaviour
{
    private Vector3 ActiveScale;
    private Vector3 InactiveScale;

    public AnimationCurve ActiveCurve;

    private float timeTweenKey, tweenValue;
    private float duration;

    public bool isActive;

    public float Speed;

    private bool tweenStarted;

    // Use this for initialization
    void Start()
    {
        tweenStarted = false;
        ActiveScale = transform.localScale;
        InactiveScale = Vector3.zero;
       // isActive = false;
        timeTweenKey = 2;

        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (timeTweenKey < 1)
            {
                timeTweenKey += Time.deltaTime / duration;
                tweenValue = ActiveCurve.Evaluate(timeTweenKey);
                transform.localScale = ActiveScale * tweenValue;
            }
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, InactiveScale, Time.deltaTime * 10);
        }
    }

    public void SetActive(bool _active, float _duration)
    {
        isActive = _active;
        duration = _duration;

        if (isActive)
        {
            if (!tweenStarted)
            {
                timeTweenKey = 0;
                tweenStarted = true;
            }
        }
        else
        {
            tweenStarted = false;
        }
        
    }

}
