﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}


    public void ShootBullet()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
        }
        rb.AddForce(Camera.main.transform.forward * GameManager.Instance.BulletForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if other if target
        if (collision.gameObject.tag.Contains("Target"))
        {
            //Target HIT!
            GameManager.Instance.Score++;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
