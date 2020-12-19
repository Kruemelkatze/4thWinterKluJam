using System;

namespace Logics
{
    [Serializable]
    public struct Stats
    {
        public int attack;
        public int armor;
        public int health;

        public Stats(Stats baseStats)
        {
            attack = baseStats.attack;
            armor = baseStats.armor;
            health = baseStats.health;
        }

        public Stats(int attack, int armor, int health)
        {
            this.attack = attack;
            this.armor = armor;
            this.health = health;
        }
    }
}