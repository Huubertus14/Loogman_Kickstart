using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DustyTextFile {

    [SerializeField]
    private readonly float duration, fadeSpeed;
    [SerializeField]
    private readonly string message;
    [SerializeField]
    private readonly AudioClip audio;

    public DustyTextFile(string _message, float _fadeSpeed, AudioClip _audio)
    {
        audio = _audio;
        duration = audio.length;
        fadeSpeed = _fadeSpeed;
        message = _message;
    }
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

    public AudioClip GetAudioClip
    {
        get {
            if (!audio)
            {
                Debug.LogError("No audio clip Attached!");
                return null;
            }

            return audio; }
    }
}
