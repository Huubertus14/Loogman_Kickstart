using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBillBoard : MonoBehaviour {

	// Update is called once per frame
	void LateUpdate () {
        transform.LookAt(Camera.main.transform.position);
	}
}
