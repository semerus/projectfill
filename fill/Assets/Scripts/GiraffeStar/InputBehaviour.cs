using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GiraffeStar
{
    /// <summary>
    /// UI 가 아닌 게임오브젝트 클릭 제어용
    /// </summary>
    public class InputBehaviour : MonoBehaviour
    {
        bool isDragging;
        float dragThreshold = 0.5f;

        public Vector3 Offset { get; private set; }

        public Action OnInputDown;
        public Action OnClick;
        //public Action onMouseDragStart;
        public Action OnDrag;
        public Action OnDragEnd;
        public Action OnInputUp;


        void OnMouseDown()
        {
            var inputPos = Input.mousePosition;
            var worldPos = Camera.main.ScreenToWorldPoint(inputPos).OverrideZ(0f);
            Offset = transform.position - worldPos;

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

        // MouseUp 시점을 EventSystem 보다 늦추기 위해 이렇게 구현
        IEnumerator OnMouseUp()
        {
            yield return new WaitForFixedUpdate();

            if(OnDragEnd != null && isDragging)
            {
                OnDragEnd();
            }

            // onclick 이 잘 작동안한다.. 나중에 한번 제대로 보자
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


