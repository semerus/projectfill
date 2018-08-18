using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GiraffeStar;
using System;

public class ColorPicker : MonoBehaviour {

    public Color CurrentColor { get; private set; }
    int currentIndex = 0;
    List<ColorItem> colorTemplates = new List<ColorItem>();

    public Color[] defaultColors = new Color[10];
    public Action OnColorChange;

	void Awake()
    {
        var buttons = GetComponentsInChildren<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {
            var colorItem = buttons[i].gameObject.GetOrAddComponent<ColorItem>();
            buttons[i].onClick.AddListener(() =>
            {
                var index = colorTemplates.IndexOf(colorItem);
                if (currentIndex == index) { return; }

                colorTemplates[currentIndex].SetState(false);
                colorItem.SetState(true);
                currentIndex = index;
                CurrentColor = colorItem.Color;

                if(OnColorChange != null)
                {
                    OnColorChange();
                }
            });
            
            colorItem.ChangeColor(defaultColors[i]);
            colorTemplates.Add(colorItem);
        }

        CurrentColor = colorTemplates[currentIndex].Color;
        colorTemplates[currentIndex].SetState(true);
    }

    public void Setup(Color original)
    {
        defaultColors[0] = original;
        if(colorTemplates.Count > 0)
        {
            colorTemplates[0].ChangeColor(original);
            colorTemplates[0].SetState(true);
        }
        
        CurrentColor = original;
    }
}
