using System;
using UnityEngine;

namespace GiraffeStar
{
    public class InputBehaviour : MonoBehaviour
    {
        bool isDragging;
        float dragThreshold = 0.5f;

        Vector3 downPosition;

        public Action OnInputDown;
        public Action OnClick;
        //public Action onMouseDragStart;
        public Action OnDrag;
        public Action OnDragEnd;
        public Action OnInputUp;

        void OnMouseDown()
        {
            //downPosition = Input.mousePosition;

            if(OnInputDown != null)
            {
                OnInputDown();
            }
        }

        void OnMouseDrag()
        {
            isDragging = true;
            if(OnDrag != null)
            {
                OnDrag();
            }
        }

        void OnMouseUp()
        {
            if(OnDragEnd != null && isDragging)
            {
                OnDragEnd();
            }

            if(OnClick != null && !isDragging)
            {
                OnClick();
            }

            isDragging = false;

            if(OnInputUp != null)
            {
                OnInputUp();
            }
        }
    }
}


