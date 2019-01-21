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

        imgGoalColor = new Color(32f/255, 81f/255, 132f/255, 1);
        textGoalColor = Color.white;
    }

    public void HoverEnter()
    {
        imgGoalColor = new Color(.2f, 1, .7f, 1);
        textGoalColor = new Color(32f / 255, 81f / 255, 132f / 255, 1);
        buttonText.fontStyle = FontStyle.Bold;
    }

    public void HoverExit()
    {
        imgGoalColor = new Color(32f / 255, 81f / 255, 132f / 255, 1);
        textGoalColor = Color.white;
        buttonText.fontStyle = FontStyle.Normal;
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
