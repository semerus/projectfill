using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableSpace : MonoBehaviour, IPointerClickHandler, IPointerUpHandler
{
    public Action<PointerEventData> onPointerClick;
    public Action<PointerEventData> onPointerUp;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(onPointerClick != null)
        {
            onPointerClick(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(onPointerUp != null)
        {
            onPointerUp(eventData);
        }
    }
}
