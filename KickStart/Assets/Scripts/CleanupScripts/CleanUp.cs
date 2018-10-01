using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanUp : MonoBehaviour {

    [Header("Destroy Values")]
    [Tooltip("Life time of the object in seconds")]
    public float LifeTime;
    [Tooltip("Delay time in seconds when the lifetime is over")]
    public float DestroyDelay;

    private float Counter;


	// Use this for initialization
	void Start () {
        Counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
        Counter += Time.deltaTime;
        if (Counter > LifeTime)
        {
            Destroy(gameObject, DestroyDelay);
        }
	}
}
