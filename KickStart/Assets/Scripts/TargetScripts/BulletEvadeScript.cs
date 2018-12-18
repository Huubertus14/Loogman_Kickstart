using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEvadeScript : MonoBehaviour {
    private TargetBehaviour birdBehaviour;

    private int evadeAmount;

    public float EvadeIncrease;

    private void Start()
    {
         evadeAmount = Random.Range(0,2);
        //evadeAmount = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Bullet")
        {
            Evade();
        }
    }
    
    private void Evade()
    {
        if (evadeAmount > 0)
        {
            Debug.Log("Bullet seen by bird");
            evadeAmount--;
            //birdBehaviour.gameObject.transform.position += new Vector3(0,0.2f,0);
            StartCoroutine(Evading());
        }
    }

    private  IEnumerator Evading()
    {
        float evadeAmount = 0;
        while (evadeAmount < 0.2f)
        {
            birdBehaviour.gameObject.transform.position += new Vector3(0, EvadeIncrease, 0);
            evadeAmount += EvadeIncrease;
            Debug.Log("Evading...");
        }
        yield return null;
    }

    public void SetBird(TargetBehaviour _behav)
    {
        birdBehaviour = _behav;
    }
}
