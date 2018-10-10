﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFox;

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
        if (collision.gameObject.tag.Contains("Target"))
        {
            //Target HIT!
            AudioManager.Instance.PlayAudio(AudioSampleManager.Instance.GetExplosionSound(), 1);
            collision.gameObject.GetComponent<TargetBehaviour>().IsHit = true;
            GameManager.Instance.Player.Score++;
            GameManager.Instance.SetScoreText();
            GameManager.Instance.Player.ScoreTextFlash.StartEffect();
            Destroy(collision.gameObject);
        }
    }
}
