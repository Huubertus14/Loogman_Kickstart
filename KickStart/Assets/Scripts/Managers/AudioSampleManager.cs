using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSampleManager : MonoBehaviour {

    public static AudioSampleManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public AudioClip Seagull;
    public AudioClip Crow;
    public AudioClip Chicken;
    public AudioClip Duck;
    public AudioClip SeagullDeluxe;
    public AudioClip CrowDeluxe;

    public AudioClip[] BirdSounds;

    public AudioClip getBirdSounds()
    {
        int x = Random.Range(0, BirdSounds.Length);
        return BirdSounds[x];
    }
    
}
