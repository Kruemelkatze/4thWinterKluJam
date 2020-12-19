using System;
using Cards.Data;
using UnityEngine;

namespace Cards
{
    public class PlayerCard : Card
    {
        [SerializeField] private PlayerCardData playerCardData;

        protected override void Awake()
        {
            base.Awake();
            Init(playerCardData, 0, false);
        }

        public override void Init(CardData data, int cn, bool destroyable = true)
        {
            cardData = data;

            canBeDestroyed = false;
            attack = cardData.attack;
            armor = cardData.armor;
            health = cardData.health;

            cardDisplay.Init();
        }
    }
}