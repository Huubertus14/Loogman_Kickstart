using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloorStabelizer : MonoBehaviour {
    
	
	void Update () {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = new Vector3(transform.position.x, -2.3f, transform.position.z);
	}
}
