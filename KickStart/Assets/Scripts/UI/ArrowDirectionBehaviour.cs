using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDirectionBehaviour : MonoBehaviour {


    // Update is called once per frame
    void Update()
    {
        if (SpawnManager.Instance.GetLastBird)
        {
            transform.LookAt(SpawnManager.Instance.GetLastBird.transform, new Vector3(1,0,0));
        }
    }
}
