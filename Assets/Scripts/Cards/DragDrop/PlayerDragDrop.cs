using System;
using Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards.DragDrop
{
    public class PlayerDragDrop : DragDrop
    {
        [Header("PlayerStuff")] [SerializeField]
        private PlayerCard playerCard;

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            GameController.Instance.playGrid.UpdateDeckDropValidities(false, true);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            GameController.Instance.playGrid.ResetDeckDropValidites(false, !hadValidDrop);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (GameController.Instance.GameState != GameState.Playing)
                return;

            base.OnDrag(eventData);
        }

        public override void SetValidDrop(int x, int y, Vector3? pos = null)
        {
            base.SetValidDrop(x, y, pos);
            playerCard.x = x;
            playerCard.y = y;
            if (pos == null)
            {
                transform.position = GameController.Instance.playGrid.GetPosition(x, y);
            }

            AudioController.Instance.PlayRandomSound("card_swipe");
        }
    }
}