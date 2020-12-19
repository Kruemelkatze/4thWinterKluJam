using System;
using System.Linq;
using Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cards.Data
{
    [CreateAssetMenu(fileName = "Card List", menuName = "Cards/Card List")]
    public class CardList : ScriptableObject
    {
        public EnemyCardData[] enemyCards;
        public PickupCardData[] pickupCards;
        public PickupCardData[] healthCards;
        public EventCardData[] eventCards;
        public DoorCardData[] doorCards;
        public TrapData[] trapCards;

        [Range(0, 1f)] public float enemyCardRatio = 0.4f;
        [Range(0, 1f)] public float pickupCardRatio = 0.2f;
        [Range(0, 1f)] public float healthCardRatio = 0.2f;
        [Range(0, 1f)] public float eventCardRatio = 0.1f;
        [Range(0, 1f)] public float trapCardRatio = 0.1f;
        [Range(0, 1f)] public float doorCardRatio = 0;

        private void OnValidate()
        {
            var pSum = enemyCardRatio + pickupCardRatio + healthCardRatio + eventCardRatio + doorCardRatio +
                       trapCardRatio;
            if (pSum == 0)
            {
                enemyCardRatio = 0.4f;
                pickupCardRatio = 0.2f;
                healthCardRatio = 0.2f;
                eventCardRatio = 0.1f;
                doorCardRatio = 0.1f;
                trapCardRatio = 0;
            }
            else
            {
                enemyCardRatio /= pSum;
                pickupCardRatio /= pSum;
                healthCardRatio /= pSum;
                eventCardRatio /= pSum;
                doorCardRatio /= pSum;
                trapCardRatio /= pSum;
            }
        }

        public (CardType type, CardData variant) GetRandom()
        {
            var type = GetRandomCardType();
            var variant = GetRandomVariantForType(type);
            return (type, variant);
        }

        public CardType GetRandomCardType()
        {
            var percentages = new[]
            {
                enemyCardRatio, pickupCardRatio, healthCardRatio,
                eventCardRatio, trapCardRatio, doorCardRatio,
            };
            var index = GetWeightedRandomEntry(percentages);

            return index switch
            {
                0 when enemyCards.Length > 0 => CardType.Enemy,
                1 when pickupCards.Length > 0 => CardType.Pickup,
                2 when healthCards.Length > 0 => CardType.Health,
                3 when eventCards.Length > 0 => CardType.Event,
                4 when trapCards.Length > 0 => CardType.Trap,
                5 when doorCards.Length > 0 => CardType.Door,
                _ => CardType.Enemy
            };
        }

        public CardData GetRandomVariantForType(CardType type)
        {
            return type switch
            {
                CardType.Enemy => enemyCards.RandomEntry(),
                CardType.Event => eventCards.RandomEntry(),
                CardType.Pickup => pickupCards.RandomEntry(),
                CardType.Health => healthCards.RandomEntry(),
                CardType.Trap => trapCards.RandomEntry(),
                CardType.Door => doorCards.RandomEntry(),
            };
        }

        #region HELPERS

        private int GetWeightedRandomEntry(float[] percentages)
        {
            var sum = percentages.Sum();
            var percentageSums = new float[percentages.Length];
            percentageSums[0] = percentages[0] / sum;

            for (int i = 1; i < percentages.Length; i++)
            {
                percentageSums[i] = percentages[i] / sum + percentageSums[i - 1];
            }

            var random = Random.Range(0f, 1f);

            var chosen = 0;
            for (int i = 0; i < percentageSums.Length; i++)
            {
                if (percentageSums[i] > random)
                {
                    return i;
                }

                chosen = i;
            }

            return chosen;
        }

        #endregion
    }
}