using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLerp : MonoBehaviour
{

    public float Speed;

    public Vector3 ActivePosition;
    public Vector3 InactivePosition;

    public bool Active;

    private Vector3 goalPos;
    public float delay;

    private RectTransform rect;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        delay = -1;
    }

    public void Update()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
            return;
        }

        rect.localPosition = Vector3.Lerp(rect.localPosition, goalPos, Time.deltaTime * Speed);

    }

    public void SetActivePosition(Vector3 _value)
    {
        ActivePosition = _value;
    }

    public void SetInactivePosition(Vector3 _value)
    {
        InactivePosition = _value;
    }

    public void SetActive(bool _value)
    {
        Active = _value;

        if (!Active)
        {
            goalPos = InactivePosition;
        }
        else
        {
            goalPos = ActivePosition;
        }
    }

    public void SetActive(bool _value, float _delay)
    {
        Active = _value;
        delay = _delay;

        if (!Active)
        {
            goalPos = InactivePosition;
        }
        else
        {
            goalPos = ActivePosition;
        }
    }

    public RectTransform GetRectTransform
    {
        get { return rect; }
    }

    public Vector3 GetActivePosition
    {
        get { return ActivePosition; }
    }
}
