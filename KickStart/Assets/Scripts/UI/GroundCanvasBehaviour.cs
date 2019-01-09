using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCanvasBehaviour : MonoBehaviour {
    
    public GameObject PlayerGroundCanvasPlace;
    private Vector3 orginPlace;

	// Use this for initialization
	void Start () {
        orginPlace = PlayerGroundCanvasPlace.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position,orginPlace, Time.deltaTime * 10);
        transform.position = new Vector3(transform.position.x, orginPlace.y,transform.position.z);
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(0,180,0);
	}
}
