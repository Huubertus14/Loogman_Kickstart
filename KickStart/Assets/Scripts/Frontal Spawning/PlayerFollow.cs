using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour {
    
	// Update is called once per frame
	void LateUpdate () {
        transform.position = Camera.main.transform.position;
	}
}
