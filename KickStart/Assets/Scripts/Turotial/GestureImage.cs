using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestureImage : MonoBehaviour
{

    private Image img;
    public Sprite[] AllImages;

    private bool isVisible;
    [Space]
    public float Duration;

    // Use this for initialization
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.GameStarted)
        {
            Duration = 3;
            isVisible = true;
        }
        else
        {
            Duration -= Time.deltaTime;
        }

        if (isVisible)
        {
            //Calculate things etc timer etc


        }
        else
        {
            img.enabled = false;
        }
    }

    public void SetInstructions(float _duration)
    {
        Duration = _duration;
    }
}
