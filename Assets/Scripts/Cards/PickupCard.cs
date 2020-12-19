using UnityEngine;

namespace Cards
{
    public class PickupCard : Card
    {
        public override (bool playerCanEnter, bool deleteThisCard) ExecuteCardAction()
        {
            var playerStats = GameController.Instance.playerCard.stats;

            playerStats.attack = Mathf.Max(1, playerStats.attack + stats.attack);
            playerStats.health = Mathf.Max(0, playerStats.health + stats.health);
            playerStats.armor = Mathf.Max(0, playerStats.armor + stats.armor);

            GameController.Instance.playerCard.UpdateStats(playerStats);

            return (true, canBeDestroyed);
        }
    }
}