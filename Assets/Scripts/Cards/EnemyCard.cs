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
            var result = FightTurn();

            return result switch
            {
                FightResult.PlayerWon => (true, canBeDestroyed),
                FightResult.EnemyWon => (false, false),
                FightResult.Draw => (false, false),
                _ => (false, false),
            };
        }
    }
}