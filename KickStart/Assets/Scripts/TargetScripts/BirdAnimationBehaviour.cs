using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAnimationBehaviour : MonoBehaviour {

    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        animator.Play("Fly");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
