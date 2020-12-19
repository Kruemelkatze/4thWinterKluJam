using UnityEngine;

namespace Logics
{
    public static class Fight
    {
        public static (Stats outPlayer, Stats outEnemy, FightResult result) FightTurn(Stats statsPlayer,
            Stats statsEnemy)
        {
            if (statsPlayer.health <= 0)
            {
                return (statsPlayer, statsEnemy, FightResult.EnemyWon);
            }

            // Player Attacks
            // Health
            var damageToEnemy = Mathf.Max(0, statsPlayer.attack - statsEnemy.armor);
            statsEnemy.health = Mathf.Max(0, statsEnemy.health - damageToEnemy);

            // Armor
            statsEnemy.armor = Mathf.Max(0, statsEnemy.armor - 1);

            // Attack
            statsPlayer.attack = Mathf.Max(1, statsPlayer.attack - 1);

            // Enemy Attacks if not dead
            if (statsEnemy.health <= 0)
                return (statsPlayer, statsEnemy, FightResult.PlayerWon);

            // Enemy Attacks
            var damageToPlayer = Mathf.Max(0, statsEnemy.attack - statsPlayer.armor);
            statsPlayer.health = Mathf.Max(0, statsPlayer.health - damageToPlayer);
            statsPlayer.armor = Mathf.Max(0, statsPlayer.armor - 1);
            //statsEnemy.attack = Mathf.Max(1, statsEnemy.attack - 1);

            var result = statsPlayer.health <= 0 ? FightResult.EnemyWon : FightResult.Draw;
            return (statsPlayer, statsEnemy, result);
        }
    }
}