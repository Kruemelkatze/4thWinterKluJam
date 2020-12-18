using System.Collections.Generic;
using Cards;
using UnityEditor;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private List<Card> cards; // down-top
    [SerializeField] private float cardOffset = 0.05f;

    [Header("Prefabs and co.")] [SerializeField]
    private GameObject cardPrefab;

    [SerializeField] private Cards.Data.Cards availableCards;

    private int _startedWithCards = 1;
    private bool _hasDoor;

    public int InitWithCards(int numberCards, bool hasDoor)
    {
        _startedWithCards = numberCards;
        _hasDoor = hasDoor;

        cards ??= new List<Card>();

        for (int i = 0; i < numberCards - 1; i++)
        {
            var offset = i * cardOffset;
            var cardGo = Instantiate(cardPrefab, transform, false);
            cardGo.transform.position += new Vector3(-offset, -offset, 0);

            var card = cardGo.GetComponent<Card>();
            card.Init(i + 1);
            cards.Add(card);
        }

        return numberCards;
    }

    private void ReShuffle()
    {
        if (cards != null)
        {
            foreach (var card in cards)
            {
                Destroy(card.gameObject);
            }

            cards.Clear();
        }

        InitWithCards(_startedWithCards, _hasDoor);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Deck))]
    private class DeckEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!(target is Deck script))
                return;

            if (Application.isPlaying && GUILayout.Button("Reshuffle"))
            {
                script.ReShuffle();
            }
        }
    }
#endif
}