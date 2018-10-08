using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDirectionBehaviour : MonoBehaviour {

    private GameObject targetBird;

    private Renderer ren;

    private void Start()
    {
        ren = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.GameStarted)
        {
            ren.enabled = false;
            return;
        }

        //if (GameManager.Instance.IsSeeingAnEnemy())
        {
          //  ren.enabled = false;
        }
       // else
        {
            if (!targetBird)
            {
                targetBird = SpawnManager.Instance.GetLastBird;
                ren.enabled = false;

            }
            else
            {
                ren.enabled = true;
                transform.LookAt(targetBird.transform, Vector3.right);
            }
        }
        
    }
}
