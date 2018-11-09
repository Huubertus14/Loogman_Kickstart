using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DustyTextFile {

    [SerializeField]
    private readonly float duration, fadeSpeed;
    [SerializeField]
    private readonly string message;

	public DustyTextFile(string _message, float _duration, float _fadeSpeed)
    {
        duration = _duration;
        fadeSpeed = _fadeSpeed;
        message = _message;
    }

    public float GetDuration
    {
        get { return duration; }
    }
    public float GetFadeSpeed
    {
        get { return fadeSpeed; }
    }
    public string GetMessage
    {
        get { return message; }
    }
}
