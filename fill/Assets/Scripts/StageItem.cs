using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace FillClient
{
    public class StageItem : MonoBehaviour, IPointerDownHandler
    {
        public float PointerDownTime { get; private set; }
        public readonly float holdThreshold = 0.7f;

        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDownTime = Time.realtimeSinceStartup;
        }
    }
}


