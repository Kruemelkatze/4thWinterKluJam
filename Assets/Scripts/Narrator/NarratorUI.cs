using System;
using TMPro;
using UnityEngine;

namespace Narrator
{
    public class NarratorUI : MonoBehaviour
    {
        [SerializeField] private Narrator narrator;
        [SerializeField] private TextMeshProUGUI subTitleField;
        [SerializeField] private RectTransform rectTransform;

        private float _initialY;

        private void Awake()
        {
            _initialY = rectTransform.position.y;
        }

        public void UpdateUI()
        {
            if (!subTitleField)
                return;

            var shouldShow = narrator.CurrentLine;
            var text = shouldShow ? narrator.CurrentLine.subtitle : null;
            text = string.IsNullOrWhiteSpace(text) ? string.Empty : $"„{text}“";

            subTitleField.SetText(text);
        }
    }
}