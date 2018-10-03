using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDirectionBehaviour : MonoBehaviour {

    private GameObject targetBird;
    private float speed = 10f;

    private Renderer ren;

    private void Start()
    {
        ren = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetBird)
        {
            targetBird = SpawnManager.Instance.GetLastBird;
            ren.enabled = false;
        }
        else
        {
            ren.enabled = true;
            transform.LookAt(targetBird.transform,Vector3.right);
        }
    }
}
