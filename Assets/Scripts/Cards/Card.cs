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

        public int attack;
        public int armor;
        public int health;


        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Init(CardData data, int cn)
        {
            cardData = data;
            cardNumber = cn;

            attack = cardData.attack;
            armor = cardData.armor;
            health = cardData.health;


            cardDisplay.Init();

            transform.name = $"({cn}) {data.name}";
        }
    }
}