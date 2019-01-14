using System.Collections;
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


        lifeTime += Time.deltaTime;
        if (lifeTime > duration)
        {
            goalColor = fadeOutColor;
        }
    }

    public void SayMessage(DustyTextFile _file)
    {
        if (_file.GetAudioClip)
        {
            //This message got an audioclip
            DustyManager.Instance.GetAudioSource.clip = _file.GetAudioClip;
            DustyManager.Instance.GetAudioSource.Play();
        }
        if (_file.GetMouthTextures.Length > 0)
        {
            StartCoroutine(DustyMouth(_file.GetAudioClip.length, _file.GetMouthTextures));
        }

        text.text = _file.GetMessage;
        duration = _file.GetDuration;
        goalColor = fadeInColor;
        lerpSpeed = _file.GetFadeSpeed;

        lifeTime = 0;
    }

    private IEnumerator DustyMouth(float _totalDuration, Texture[] _textures)
    {
        float _imageInterval = _totalDuration / _textures.Length;

        for (int i = 0; i < _textures.Length; i++)
        {
            DustyManager.Instance.SetDustyMouthTexture(_textures[i]);
            yield return new WaitForSeconds(_imageInterval);
        }

        yield return null;
    }

    public void StopMessage()
    {
        DustyManager.Instance.GetAudioSource.Stop();
        text.text = "";
        lifeTime = 0;
    }

    public bool CanSayNextSentence
    {
        get
        {
            //Calculate if message is empty
            if (lifeTime < duration + 0.4f)
            {
                return false;
            }
            return true;
        }
    }

}
