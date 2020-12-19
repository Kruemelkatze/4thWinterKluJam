using System;
using DG.Tweening;
using Extensions;
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

        [SerializeField] private Image attackUp;
        [SerializeField] private Image attackDown;
        [SerializeField] private Image armorUp;
        [SerializeField] private Image armorDown;
        [SerializeField] private Image healthUp;
        [SerializeField] private Image healthDown;

        [Header("Backside")] [SerializeField] private Image cardBackBody;
        [SerializeField] private TextMeshProUGUI cardBackDescription;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            ResetPreview(true);
        }

        public void Show(bool value, bool instant = false)
        {
            _canvasGroup.Fade(value, instant);
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
            UpdateFields(card.stats);
        }

        public void UpdateFields(Stats stats)
        {
            ResetPreview();
            SetFields(stats);
        }

        private void SetFields(Stats stats)
        {
            if (attackField)
                attackField.SetText(stats.attack.ToString());

            if (armorField)
                armorField.SetText(stats.armor.ToString());

            if (healthField)
                healthField.SetText(stats.health.ToString());
        }

        public void ResetPreview(bool instant = false)
        {
            if (attackUp)
                attackUp.Fade(false, instant);

            if (attackDown)
                attackDown.Fade(false, instant);

            if (armorUp)
                armorUp.Fade(false, instant);

            if (armorDown)
                armorDown.Fade(false, instant);

            if (healthUp)
                healthUp.Fade(false, instant);

            if (healthDown)
                healthDown.Fade(false, instant);
        }

        public void ShowPreview(Stats previewStats)
        {
            var attackDiff = previewStats.attack - card.stats.attack;
            var armorDiff = previewStats.armor - card.stats.armor;
            var healthDiff = previewStats.health - card.stats.health;

            SetFields(previewStats);

            if (attackUp && attackDiff > 0)
                attackUp.Fade(true);

            if (attackDown && attackDiff < 0)
                attackDown.Fade(true);

            if (armorUp && armorDiff > 0)
                armorUp.Fade(true);

            if (armorDown && armorDiff < 0)
                armorDown.Fade(true);

            if (healthUp && healthDiff > 0)
                healthUp.Fade(true);

            if (healthDown && healthDiff < 0)
                healthDown.Fade(true);
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
    }
}