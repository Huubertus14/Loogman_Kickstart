using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCanvasBehaviour : MonoBehaviour {

    Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Camera.main.transform.position + offset;
	}
}
