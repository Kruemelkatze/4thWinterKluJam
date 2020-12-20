using Logics;
using UnityEngine;

namespace Cards
{
    public class PickupCard : Card
    {
        public override (bool playerCanEnter, bool deleteThisCard) ExecuteCardAction()
        {
            visited++;

            var playerStats = GetPlayerStatsAfterPickup();
            GameController.Instance.playerCard.UpdateStats(playerStats);

            PlayAudioLine();

            CardSolved();

            return (true, canBeDestroyed);
        }

        private Stats GetPlayerStatsAfterPickup()
        {
            var playerStats = GameController.Instance.playerCard.stats;

            playerStats.attack = Mathf.Max(1, playerStats.attack + stats.attack);
            playerStats.health = Mathf.Max(0, playerStats.health + stats.health);
            playerStats.armor = Mathf.Max(0, playerStats.armor + stats.armor);

            return playerStats;
        }

        public override (Stats ownStats, Stats playerStats) GetPreviewStats()
        {
            var playerStats = GetPlayerStatsAfterPickup();
            return (stats, playerStats);
        }
    }
}