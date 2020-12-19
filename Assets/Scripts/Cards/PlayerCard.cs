using Cards.Data;
using UnityEngine;

namespace Cards
{
    public class PlayerCard : Card
    {
        [SerializeField] private PlayerCardData playerCardData;

        public int x;
        public int y;

        protected override void Awake()
        {
            base.Awake();
            Init(playerCardData, 0, false);

            var (pos, x, y) = GameController.Instance.playGrid.GetPlayerSpawnPosition();
            transform.position = pos;
            this.x = x;
            this.y = y;
        }

        public override void Init(CardData data, int cn, bool destroyable = true)
        {
            cardData = data;

            canBeDestroyed = false;
            stats.attack = cardData.attack;
            stats.armor = cardData.armor;
            stats.health = cardData.health;

            cardDisplay.Init();
            cardDisplay.ShowFields(true, true, true);
        }
    }
}