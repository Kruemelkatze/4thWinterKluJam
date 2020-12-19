using Logics;

namespace Cards
{
    public class EnemyCard : Card
    {
        public FightResult FightTurn()
        {
            var statsPlayer = GameController.Instance.playerCard.stats;
            var enemyStats = stats;
            FightResult result;

            (statsPlayer, enemyStats, result) = Fight.FightTurn(statsPlayer, enemyStats);

            UpdateStats(enemyStats);
            GameController.Instance.playerCard.UpdateStats(statsPlayer);

            return result;
        }

        public override (bool playerCanEnter, bool deleteThisCard) ExecuteCardAction()
        {
            visited++;

            var result = FightTurn();

            var output = result switch
            {
                FightResult.PlayerWon => (true, canBeDestroyed),
                FightResult.EnemyWon => (false, false),
                FightResult.Draw => (false, false),
                _ => (false, false),
            };

            if (result == FightResult.PlayerWon && canBeDestroyed)
            {
                AddPointsAfterSolved(true);
            }

            return output;
        }

        public override void Init(CardData data, int cn, bool destroyable = true)
        {
            base.Init(data, cn, destroyable);
            cardDisplay.ShowFields(true, true, true);
        }

        public override (Stats ownStats, Stats playerStats) GetPreviewStats()
        {
            var statsPlayer = GameController.Instance.playerCard.stats;
            var enemyStats = stats;
            FightResult result;

            (statsPlayer, enemyStats, result) = Fight.FightTurn(statsPlayer, enemyStats);

            return (enemyStats, statsPlayer);
        }
    }
}