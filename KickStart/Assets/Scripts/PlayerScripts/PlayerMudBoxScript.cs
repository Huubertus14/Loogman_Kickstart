using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMudBoxScript : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        //Bird trows dirt at you
        if (other.gameObject.tag.Contains("Target"))
        {
            Debug.Log("YEET");
            other.gameObject.GetComponent<TargetBehaviour>().TrowGarbage();
            GameManager.Instance.Player.GetMud();
        }
        Debug.Log("YEET@2");
    }
}
