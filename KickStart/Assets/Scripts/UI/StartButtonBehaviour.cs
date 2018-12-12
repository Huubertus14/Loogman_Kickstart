using UnityEngine;
using UnityEngine.UI;

public class StartButtonBehaviour : MonoBehaviour
{

    Image buttonImg;
    Text buttonText;

    Color imgGoalColor, textGoalColor;

    private void Start()
    {
        buttonImg = GetComponent<Image>();
        buttonText = GetComponentInChildren<Text>();

        imgGoalColor = Color.white;
        textGoalColor = Color.white;
    }

    public void HoverEnter()
    {
        imgGoalColor = new Color(.2f, 1, .7f, 1);
        textGoalColor = Color.cyan;
    }

    public void HoverExit()
    {
        imgGoalColor = Color.white;
        textGoalColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        HandleColor();
    }

    private void HandleColor()
    {
        buttonImg.color = Color.Lerp(buttonImg.color, imgGoalColor, Time.deltaTime * 10);
        buttonText.color = Color.Lerp(buttonText.color, textGoalColor, Time.deltaTime * 10);
    }
}
