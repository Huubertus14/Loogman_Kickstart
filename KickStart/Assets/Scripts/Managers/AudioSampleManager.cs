using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSampleManager : MonoBehaviour {

    public static AudioSampleManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public AudioClip Seagulls;
    
}
