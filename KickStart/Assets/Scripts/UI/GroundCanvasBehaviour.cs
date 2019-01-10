using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCanvasBehaviour : MonoBehaviour {

    public GameObject PlayerOrientation;
    private Vector3 offset;
	// Use this for initialization
	void Start () {
        offset = new Vector3(0,-5,2);
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = PlayerOrientation.transform.forward * 5f;
        transform.position += offset;

        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(0,180,0);
	}
}
