using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollector : MonoBehaviour {


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Contains("Garbage"))
        {
            Debug.Log("Garbage fell on you!");
            GameManager.Instance.HitByGarbage++;
            GameManager.Instance.SetGarbageText();
            GameManager.Instance.Player.GarbageTextFlash.StartEffect();
            other.gameObject.transform.SetParent(transform);

            //Destroy(other.gameObject, 1.5f);
        }
    }
}
