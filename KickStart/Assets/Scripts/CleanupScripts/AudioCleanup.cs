using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCleanup : MonoBehaviour {

    private AudioSource source;
    private bool isChecking;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isChecking)
        {
            if (!source.isPlaying)
            {
                Destroy(gameObject);
            }
        }
    }

    public void StartChecking()
    {
        isChecking = true;
    }
}
