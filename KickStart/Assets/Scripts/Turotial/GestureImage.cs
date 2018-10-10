using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VrFox;

public class GestureImage : MonoBehaviour
{

    private Image img;
    public Sprite[] AllImages;

    public bool IsVisible;
    [Space]
    public float Duration;

    private float intervalTimer;
    private int imageCount;

    // Use this for initialization
    void Start()
    {
        img = GetComponent<Image>();
        
        imageCount = 0;
        img.sprite = AllImages[imageCount];
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.GameStarted)
        {
            Duration = 3;
            IsVisible = true;
        }
        else
        {
            Duration -= Time.deltaTime;
            IsVisible = false;
        }

        if (IsVisible)
        {
            img.enabled = true;
            //Calculate things etc timer etc
            intervalTimer += Time.deltaTime;
            if (intervalTimer > 0.4f)
            {
                intervalTimer = 0;
                //Increase the image count

                imageCount++;
                if (imageCount > 1)
                {
                    imageCount = 0;
                }
                img.sprite = AllImages[imageCount];

            }

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
