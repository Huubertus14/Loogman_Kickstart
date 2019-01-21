using UnityEngine;
using VrFox;

[System.Serializable]
public class DustyTextFile
{

    [SerializeField]
    private readonly float duration, fadeSpeed;
    [SerializeField]
    private readonly string message;
    [SerializeField]
    private readonly AudioClip audio;
    [SerializeField]
    private readonly Texture[] mouthTextures;

    public DustyTextFile(string _message, float _fadeSpeed, AudioClip _audio)
    {
        audio = _audio;
        duration = audio.length;
        fadeSpeed = _fadeSpeed;
        message = _message;
        mouthTextures = DustyManager.Instance.MouthSequence.GetText01;
    }
    

    public float GetDuration => duration;
    public float GetFadeSpeed => fadeSpeed;
    public string GetMessage => message;
    public Texture[] GetMouthTextures => mouthTextures;

    public AudioClip GetAudioClip
    {
        get
        {
            if (!audio)
            {
                //  Debug.LogError("No audio clip Attached!");
                return null;
            }

            return audio;
        }
    }
}
