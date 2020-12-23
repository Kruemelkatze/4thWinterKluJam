using System;

namespace Logics
{
    [Serializable]
    public struct Stats
    {
        public int attack;
        public int armor;
        public int health;
        public int tempArmor; //temp armor added

        public Stats(Stats baseStats)
        {
            attack = baseStats.attack;
            armor = baseStats.armor;
            health = baseStats.health;
            tempArmor = baseStats.tempArmor; //temp armor added
        }

        public Stats(int attack, int armor, int health, int tempArmor)
        {
            this.attack = attack;
            this.armor = armor;
            this.health = health;
            this.tempArmor = tempArmor; //temp added
        }
    }
}