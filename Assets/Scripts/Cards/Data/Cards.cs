using UnityEngine;

namespace Cards.Data
{
    [CreateAssetMenu(fileName = "Card List", menuName = "Cards/Card List")]
    public class Cards : ScriptableObject
    {
        public EnemyCardData[] enemyCards;
        public PickupCardData[] pickupCards;
        public BlankCardData[] blankCards;
        public DoorCardData[] doorCards;
        public TrapData[] trapCards;
    }
}