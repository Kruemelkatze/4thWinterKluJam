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
            statsEnemy.tempArmor = statsEnemy.armor; //sets armor value as tempArmor
            statsEnemy.armor = Mathf.Max(0, statsEnemy.armor - statsPlayer.attack);

            // Attack
            if (statsEnemy.armor != statsEnemy.tempArmor) //compares armor with tempArmor
            {
                statsPlayer.attack = Mathf.Max(1, statsPlayer.attack - 1); //reduces attack if mosnter has armour
            }


            // Enemy Attacks if not dead
            if (statsEnemy.health <= 0)
                return (statsPlayer, statsEnemy, FightResult.PlayerWon);

            // Enemy Attacks
            statsPlayer.tempArmor = statsPlayer.armor; //sets armor value as tempArmor
            var damageToPlayer = Mathf.Max(0, statsEnemy.attack - statsPlayer.armor);
            statsPlayer.health = Mathf.Max(0, statsPlayer.health - damageToPlayer);
            statsPlayer.armor = Mathf.Max(0, statsPlayer.armor - statsEnemy.attack);

            if (statsPlayer.armor != statsPlayer.tempArmor) // compares armor with tempArmor
            {
                statsEnemy.attack = Mathf.Max(0, statsEnemy.attack - 1); // reduces attack if player has armour
            }

            var result = statsPlayer.health <= 0 ? FightResult.EnemyWon : FightResult.Draw;
            return (statsPlayer, statsEnemy, result);
        }
    }
}