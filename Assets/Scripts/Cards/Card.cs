using System;
using Extensions;
using UnityEngine;

namespace Cards
{
    public class Card : MonoBehaviour
    {
        [SerializeField] protected CardDisplay cardDisplay;

        [ReadOnly] public int cardNumber;
        [ReadOnly] public CardData cardData;
        
        public bool canBeDestroyed = true;

        public int attack;
        public int armor;
        public int health;

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
            attack = cardData.attack;
            armor = cardData.armor;
            health = cardData.health;


            cardDisplay.Init();

            transform.name = $"({cn}) {data.name}";
        }
    }
}