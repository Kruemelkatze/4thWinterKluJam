using System;
using Extensions;
using Logics;
using UnityEngine;

namespace Cards
{
    public class Card : MonoBehaviour
    {
        [SerializeField] protected CardDisplay cardDisplay;

        [ReadOnly] public int cardNumber;
        [ReadOnly] public CardData cardData;

        public bool canBeDestroyed = true;

        public Stats stats;

        protected virtual void Awake()
        {
            SetupCanvases();
        }

        protected void SetupCanvases()
        {
            var canvases = GetComponentsInChildren<Canvas>();
            foreach (var canvas in canvases)
            {
                canvas.worldCamera = Camera.main;
            }
        }

        public virtual void Init(CardData data, int cn, bool destroyable = true)
        {
            cardData = data;
            cardNumber = cn;

            canBeDestroyed = destroyable;
            stats.attack = cardData.attack;
            stats.armor = cardData.armor;
            stats.health = cardData.health;


            cardDisplay.Init();

            transform.name = $"({cn}) {data.name}";
        }

        public virtual void UpdateStats(Stats newStats)
        {
            stats = newStats;
            cardDisplay.UpdateFields();
        }

        public virtual (bool playerCanEnter, bool deleteThisCard) ExecuteCardAction()
        {
            return (true, canBeDestroyed);
        }

        public void DestroyCard()
        {
            Destroy(gameObject);
        }
    }
}