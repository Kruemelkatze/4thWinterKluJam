using Extensions;
using Logics;
using UnityEngine;

namespace Cards
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private bool isPreviewCard = false;

        [SerializeField] protected CardDisplay cardDisplay;

        [ReadOnly] public int cardNumber;
        [ReadOnly] public CardData cardData;

        public bool canBeDestroyed = true;

        public Stats stats;
        public int visited = 0;

        protected virtual void Awake()
        {
            SetupCanvases();

            if (isPreviewCard)
            {
                cardDisplay.Init();
            }
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
            if (isPreviewCard)
                return;

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
            visited++;
            AddPointsAfterSolved();
            return (true, canBeDestroyed);
        }

        protected void AddPointsAfterSolved(bool always = false)
        {
            if (always || visited == 1)
            {
                GameController.Instance.heroPoints += cardData.pointsOnSolve;
            }
        }

        public void DestroyCard()
        {
            Destroy(gameObject);
        }

        public void ResetPreview()
        {
            cardDisplay.UpdateFields();
        }

        public void ShowPreview(Stats previewStats)
        {
            cardDisplay.ShowPreview(previewStats);
        }

        public virtual (Stats ownStats, Stats playerStats) GetPreviewStats()
        {
            return (stats, GameController.Instance.playerCard.stats);
        }
    }
}