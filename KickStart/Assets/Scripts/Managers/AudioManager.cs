﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

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
    public void PlayAudio(AudioClip _clip, float _volume, GameObject _sourceObject)
    {
        if (!_clip)
        {
            Debug.LogError("No Audioclip attached!");
            return;
        }

        AudioSource _source;

        //clamp volume
        _volume = Mathf.Clamp01(_volume);

        //Check if the current object has a audiosource
        if (_sourceObject.GetComponent<AudioSource>())
        {
            _source = _sourceObject.GetComponent<AudioSource>();

            _source.clip = _clip;
            _source.volume = _volume;

            if (!_source.isPlaying)
            {
                _source.Play();
            }

            return;
        }
        else
        {
            PlayAudio(_clip, _volume);
        }

    }

    /// <summary>
    /// Call this when there is no 3d sound 
    /// </summary>
    public void PlayAudio(AudioClip _clip, float _volume)
    {
        if (!_clip)
        {
            Debug.LogError("No Audioclip attached!");
            return;
        }

        //Create the audio object
        GameObject _audioSource = Instantiate(AudioPrefab, Camera.main.transform.position, Quaternion.identity);
        AudioSource _source = _audioSource.GetComponent<AudioSource>();

        _volume = Mathf.Clamp01(_volume);

        //Set the values
        _source.clip = _clip;
        _source.volume = _volume;

        //Play the audio
        _source.Play();
        _audioSource.GetComponent<AudioCleanup>().StartChecking();
    }
}
