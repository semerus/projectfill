using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableSpace : MonoBehaviour, IPointerClickHandler
{
    public Action OnPointerClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(OnPointerClicked != null)
        {
            OnPointerClicked();
        }
    }
}
