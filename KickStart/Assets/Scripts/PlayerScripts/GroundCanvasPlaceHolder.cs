using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCanvasPlaceHolder : MonoBehaviour {

    Vector3 orginLocalPosition;

	// Use this for initialization
	void Start () {
        orginLocalPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(transform.position.x, orginLocalPosition.y, transform.position.z);
	}
}
