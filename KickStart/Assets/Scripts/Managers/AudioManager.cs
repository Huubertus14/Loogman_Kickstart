using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("Audio Prefabs:")]
    public GameObject AudioPrefab; //The prefab that generates the audio

    /// <summary>
    /// Called when there is a sound object on a specific position
    /// </summary>
    public void PlayAudio(AudioClip _clip, float _volume, Vector3 _position)
    {
        GameObject _audioSource = Instantiate(AudioPrefab, _position, Quaternion.identity);
        AudioSource _source = _audioSource.GetComponent<AudioSource>();
        _source.clip = _clip;
        _source.volume = _volume;

        _source.Play();
        _audioSource.GetComponent<AudioCleanup>().StartChecking();
    }

    /// <summary>
    /// Call this when there is no 3d sound 
    /// </summary>
    public void PlayAudio(AudioClip _clip, float _volume)
    {
        GameObject _audioSource = Instantiate(AudioPrefab, Camera.main.transform.position, Quaternion.identity);
        AudioSource _source = _audioSource.GetComponent<AudioSource>();
        _source.clip = _clip;
        _source.volume = _volume;

        _source.Play();
        _audioSource.GetComponent<AudioCleanup>().StartChecking();
    }
}
