using System;
using Cards.Data;
using Logics;
using UnityEngine;

namespace Cards
{
    public class PlayerCard : Card
    {
        [SerializeField] private PlayerCardData playerCardData;

        public int x = -1000;
        public int y = -1000;

        protected override void Awake()
        {
            base.Awake();

            x = -1000;
            y = -1000;

            Init(null, playerCardData, 0, false);

            Show(false, true);
        }

        public void SetPosition(int x, int y, Vector3 pos)
        {
            this.x = x;
            this.y = y;
            SetPosition(pos);
        }

        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }

        public override void Init(Deck deck, CardData data, int cn, bool destroyable = true)
        {
            this.deck = deck;
            cardData = data;

            canBeDestroyed = false;
            stats.attack = cardData.attack;
            stats.armor = cardData.armor;
            stats.health = cardData.health;

            cardDisplay.Init();
            cardDisplay.ShowFields(true, true, true);
        }

        public override void UpdateStats(Stats newStats)
        {
            base.UpdateStats(newStats);
            if (stats.health <= 0)
            {
                GameController.Instance.PlayerHealthReachedZero();
            }
        }
    }
}