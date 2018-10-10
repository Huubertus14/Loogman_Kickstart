using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustyManager : MonoBehaviour
{

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

    Animation ani;

    float headSpinTimer;
    private void Start()
    {
        ani = Head.GetComponent<Animation>();
    }

    private void Update()
    {
        headSpinTimer += Time.deltaTime;
        if (headSpinTimer > 15)
        {
            headSpinTimer = 0;
            ani.Play();
        }

    }
}
