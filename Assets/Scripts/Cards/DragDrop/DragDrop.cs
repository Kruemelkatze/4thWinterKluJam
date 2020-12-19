using System;
using Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards.DragDrop
{
    public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private float dragAlpha = 0.7f;
        [ReadOnly] [SerializeField] private bool dragging;
        [ReadOnly] [SerializeField] private bool hadValidDrop;
        [ReadOnly] [SerializeField] private Vector3 dragStartPosition;


        protected CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            dragStartPosition = transform.position;

            hadValidDrop = false;
            dragging = true;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = dragAlpha;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            dragging = false;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;

            if (!hadValidDrop)
            {
                transform.position = dragStartPosition;
            }

            hadValidDrop = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            dragging = true;

            Vector3 screenCenter =
                new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, -Camera.main.transform.position.z);
            Vector3 screenTouch = screenCenter + new Vector3(eventData.delta.x, eventData.delta.y, 0);

            Vector3 worldCenterPosition = Camera.main.ScreenToWorldPoint(screenCenter);
            Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(screenTouch);

            Vector3 delta = worldTouchPosition - worldCenterPosition;
            transform.position += delta;
        }

        public void SetValidDrop()
        {
            hadValidDrop = true;
        }
    }
}