using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeScript : MonoBehaviour
{

    private Color fadeOutColor;
    public Color FadeInColor;
    private Color goalColor;

    public float Speed;

    private Text fadeText;

    // Use this for initialization
    void Start()
    {
        fadeText = GetComponent<Text>();
        fadeOutColor = new Color(0, 0, 0, 0);
        fadeText.color = fadeOutColor;
        Speed = 3;
    }

    // Update is called once per frame
    void Update()
    {
        fadeText.color = Color.Lerp(fadeText.color, goalColor, Time.deltaTime * Speed);
    }

    public void Active(bool _value)
    {
        if (_value)
        {
            goalColor = FadeInColor;
        }
        else
        {
            goalColor = fadeOutColor;
        }
    }
}
