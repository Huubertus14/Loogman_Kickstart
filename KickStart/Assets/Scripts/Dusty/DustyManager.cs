using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustyManager : MonoBehaviour {

    public static DustyManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("Refs:")]
    public GameObject Head;
    public GameObject LeftArm;
    public GameObject RightArm;
    public GameObject Torso;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Head.GetComponent<Animation>().Play();
        }
    }
}
