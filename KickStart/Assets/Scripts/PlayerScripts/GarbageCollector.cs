using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollector : MonoBehaviour {
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Contains("Garbage"))
        {
            Debug.Log("Garbage fell on you!");
            GameManager.Instance.Player.HitByGarbage++;
            other.gameObject.transform.SetParent(transform);
        }
    }
}
