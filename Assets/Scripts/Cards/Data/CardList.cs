﻿using System;
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

            switch (index)
            {
                case 0 when enemyCards.Length > 0:
                    return CardType.Enemy;
                case 1 when pickupCards.Length > 0:
                    return CardType.Pickup;
                case 2 when healthCards.Length > 0:
                    return CardType.Health;
                case 3 when eventCards.Length > 0:
                    return CardType.Event;
                case 4 when trapCards.Length > 0:
                    return CardType.Trap;
                case 5 when doorCards.Length > 0:
                    return CardType.Door;
                default:
                    return CardType.Enemy;
            }
        }

        public CardData GetRandomVariantForType(CardType type)
        {
            switch (type)
            {
                case CardType.Enemy:
                    return enemyCards.RandomEntry();
                case CardType.Event:
                    return eventCards.RandomEntry();
                case CardType.Pickup:
                    return pickupCards.RandomEntry();
                case CardType.Health:
                    return healthCards.RandomEntry();
                case CardType.Trap:
                    return trapCards.RandomEntry();
                case CardType.Door:
                    return doorCards.RandomEntry();
                default:
                    return eventCards.RandomEntry();
            }
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