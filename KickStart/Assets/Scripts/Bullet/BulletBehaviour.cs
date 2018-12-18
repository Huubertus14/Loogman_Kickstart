﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFox;

public class BulletBehaviour : MonoBehaviour
{
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ShootBullet()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
        }
        rb.AddForce(Camera.main.transform.forward * GameManager.Instance.GetBulletForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Target"))
        {
            //Target HIT!
            AudioManager.Instance.PlayAudio(AudioSampleManager.Instance.GetExplosionSound(), 1);
            collision.gameObject.GetComponent<TargetBehaviour>().Hit();
            
            GameManager.Instance.SetScoreText();

            Destroy(gameObject);
        }
        if (collision.gameObject.tag.Contains("Dusty"))
        {
            //REEEEEEEEEEEEEEEEEEEEEEEEEE
        }
    }
}
