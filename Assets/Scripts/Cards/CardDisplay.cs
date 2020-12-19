using Logics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    public class CardDisplay : MonoBehaviour
    {
        [SerializeField] private Card card;

        [Header("Frontside")] [SerializeField] private Image cardBody;
        [SerializeField] private Image cardIcon;
        [SerializeField] private TextMeshProUGUI cardName;
        [SerializeField] private TextMeshProUGUI frontDescription;
        [SerializeField] private TextMeshProUGUI attackField;
        [SerializeField] private TextMeshProUGUI armorField;
        [SerializeField] private TextMeshProUGUI healthField;

        [Header("Backside")] [SerializeField] private Image cardBackBody;
        [SerializeField] private TextMeshProUGUI cardBackDescription;


        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void ShowFields(bool attack, bool armor, bool health)
        {
            if (attackField)
                attackField.enabled = attack;

            if (armorField)
                armorField.enabled = armor;

            if (healthField)
                healthField.enabled = health;
        }

        public void UpdateFields()
        {
            if (attackField)
                attackField.SetText(card.stats.attack.ToString());

            if (armorField)
                armorField.SetText(card.stats.armor.ToString());

            if (healthField)
                healthField.SetText(card.stats.health.ToString());
        }

        public void Init()
        {
            var canvases = GetComponentsInChildren<Canvas>();
            foreach (var canvas in canvases)
            {
                canvas.sortingOrder += card.cardNumber;
            }

            var data = card.cardData;
            // if (cardBody)
            //     cardBody.sprite = ...

            if (cardIcon)
            {
                cardIcon.sprite = data.icon;
                cardIcon.color = Color.white;
            }

            if (cardName)
                cardName.SetText(data.name);

            UpdateFields();
            ShowFields(card.stats.attack != 0, card.stats.armor != 0, card.stats.health != 0);

            // if (cardBackBody)
            //     cardBackBody.

            if (cardBackDescription)
                cardBackDescription.SetText(data.text);

            if (frontDescription)
                frontDescription.SetText(data.text);
        }

        public void PreviewStats(Stats stats)
        {
        }
    }
}