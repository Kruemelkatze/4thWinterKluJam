using Extensions;
using UnityEngine;

namespace Cards
{
    public class DoorCard : Card
    {
        public override (bool playerCanEnter, bool deleteThisCard) ExecuteCardAction()
        {
            visited++;
            CardSolved();
            GameController.Instance.DoorReached(deck.x, deck.y);
            PlayAudioLine();

            return (true, false);
        }
    }
}