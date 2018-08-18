using UnityEngine;
using UnityEngine.UI;
using GiraffeStar;

public class ColorItem : MonoBehaviour {

    public Color Color { get; private set; }
    Image colorButton;
    Image border;

    void Awake()
    {
        colorButton = GetComponent<Image>();
        border = gameObject.FindChildByName("Border").GetComponent<Image>();
        SetState(false);
    }

    public void ChangeColor(Color color)
    {
        Color = color;
        colorButton.color = color;
    }
    
    public void SetState(bool isOn)
    {
        if(isOn)
        {
            border.color = Color.cyan;
        }
        else
        {
            border.color = Color.gray;
        }
    }
}
