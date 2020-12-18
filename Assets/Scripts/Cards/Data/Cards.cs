using UnityEngine;

namespace Cards.Data
{
    public class Cards : ScriptableObject
    {
        public EnemyCardData enemyCards;
        public PickupCardData pickupCards;
        public BlankCardData blankCards;
        public DoorCardData doorCards;
        public TrapData trapCards;
    }
}