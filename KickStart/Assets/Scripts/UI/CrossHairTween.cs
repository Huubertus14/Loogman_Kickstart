using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairTween : MonoBehaviour
{
    public Vector3 ActiveScale;
    public Vector3 InactiveScale;

    public AnimationCurve ActiveCurve;
    public AnimationCurve InactiveCurve;

    public AnimationCurve JiggleAnimation;
    private float jiggleTween, jiggleTimeKey, jiggleDuration;

    private float timeTweenKey, tweenValue;
    private float duration;

    public bool isActive;

    public float Speed;

    // Use this for initialization
    void Start()
    {
        jiggleTimeKey = 2;
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
            else
            {
                Jiggle();
            }
        }
        else
        {
            if (timeTweenKey < 1)
            {
                timeTweenKey += Time.deltaTime / duration;
                tweenValue = ActiveCurve.Evaluate(timeTweenKey);
                transform.localScale = InactiveScale * tweenValue;
            }
        }
    }

    private void Jiggle()
    {
        if (jiggleTimeKey < 1)
        {
            jiggleTimeKey += Time.deltaTime / jiggleDuration;
            jiggleTween = JiggleAnimation.Evaluate(jiggleTimeKey);
            transform.localScale = ActiveScale * jiggleTween;
        }
    }

    /// <summary>
    /// Call this to start a jiggle of the crosshair
    /// </summary>
    /// <param the duration of the jiggle (keep it short)="_duration"></param>
    public void StartJiggle(float _duration)
    {
        jiggleDuration = _duration;
        jiggleTimeKey = 0;
    }

    public void SetActive(bool _active, float _duration)
    {
        isActive = _active;
        duration = _duration;

        if (isActive)
        {
            timeTweenKey = 0;
        }
    }
}
