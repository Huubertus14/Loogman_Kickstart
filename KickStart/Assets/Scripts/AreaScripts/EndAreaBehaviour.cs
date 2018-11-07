using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFox;

public class EndAreaBehaviour : MonoBehaviour {
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerBehaviour>())
        {
            Debug.Log("End Reached!");
        }
    }
}
