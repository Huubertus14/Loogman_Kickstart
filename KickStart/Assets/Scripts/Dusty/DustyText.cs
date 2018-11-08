using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustyText : MonoBehaviour
{

    private Color fadeOutColor = new Color(1, 1, 1, 0);
    private Color fadeInColor = Color.white;
    private Color goalColor;

    private TextMesh text;
    private float duration;
    private float lerpSpeed;
    private float lifeTime;
    

    // Use this for initialization
    void Start()
    {
        text = GetComponent<TextMesh>();


        goalColor = fadeOutColor;
        text.color = fadeOutColor;

        text.text = "";
        lerpSpeed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        text.color = Color.Lerp(text.color, goalColor, Time.deltaTime * lerpSpeed);
        
        if (lifeTime > duration)
        {
            goalColor = fadeOutColor;
        }
        else
        {
            lifeTime += Time.deltaTime;
        }
    }

    public void SayMessage(DustyTextFile _file)
    {
        text.text = _file.GetMessage;
        duration = _file.GetDuration;
        goalColor = fadeInColor;
        lerpSpeed = _file.GetFadeSpeed;

        //AudioManager.Instance.PlayAudio(AudioSampleManager.Instance.GetDustyTalkSound(), 1, gameObject);

        lifeTime = 0;
    }

    public bool CanSayNextSentence
    {
        get
        {
            //Calculate if message is empty
            if (lifeTime < duration)
            {
                return false;
            }
            return true;
        }
    }

}
