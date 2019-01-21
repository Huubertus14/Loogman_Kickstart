using UnityEngine;

public class DustyBlink : MonoBehaviour
{

    public AnimationCurve BlinkCurve;


    private Vector3 orginScale = new Vector3(0.25f, 0.25f, 0.25f);

    float tweenValue, timeTweenKey, duration;

    // Use this for initialization
    void Start()
    {
        timeTweenKey = 2;
    }
    Vector3 goal;
    // Update is called once per frame
    void Update()
    {
        if (timeTweenKey < 1)
        {
            timeTweenKey += Time.deltaTime / duration;
            tweenValue = BlinkCurve.Evaluate(timeTweenKey);
            goal = new Vector3(0.25f, 0.25f, 0.25f * tweenValue);
        }
        else
        {
           goal = orginScale;
        }
        transform.localScale = Vector3.Lerp(transform.localScale, goal, Time.deltaTime * 10);
    }

    public void Blink(float _dur)
    {
        timeTweenKey = 0;
        duration = _dur;
    }
}
