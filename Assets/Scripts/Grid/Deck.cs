using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.Data;
using Cards.DragDrop;
using DG.Tweening;
using Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Deck : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SpriteRenderer notAvailableHover;
    [SerializeField] private float fadeTime = 0.3f;

    [SerializeField] private float cardOffset = 0.05f;
    [SerializeField] private List<Card> cards; // down-top


    public int x;
    public int y;

    [Header("Prefabs and co.")] [SerializeField]
    private CardTypeGameObjectDictionary cardPrefabs = new CardTypeGameObjectDictionary();

    private CardList _availableCards;
    private int _startedWithCards = 1;
    private bool _hasDoor;

    public int InitWithCards(CardList availableCards, int numberCards, bool hasDoor)
    {
        _availableCards = availableCards;
        _startedWithCards = numberCards;
        _hasDoor = hasDoor;

        cards ??= new List<Card>();

        for (var i = 0; i < numberCards; i++)
        {
            CardType? type = i != 0 ? (CardType?) null : (hasDoor ? CardType.Door : CardType.Trap);
            var card = SpawnCard(i, type, i != 0);
            cards.Add(card);
        }


        return numberCards;
    }

    private Card SpawnCard(int number, CardType? type = null, bool destroyable = true)
    {
        var offset = number * cardOffset;

        CardType cardType;
        CardData cardVariant;
        if (type == null)
        {
            (cardType, cardVariant) = _availableCards.GetRandom();
        }
        else
        {
            cardType = type.Value;
            cardVariant = _availableCards.GetRandomVariantForType(cardType);
        }

        var prefab = cardPrefabs[cardType];

        var cardGo = Instantiate(prefab, transform, false);
        var card = cardGo.GetComponent<Card>();
        card.Init(cardVariant, number + 1, destroyable);

        card.transform.position += new Vector3(-offset, -offset, 0);


        return card;
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

        InitWithCards(_availableCards, _startedWithCards, _hasDoor);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (GameController.Instance.GameState != GameState.Playing)
            return;

        var draggedObj = eventData.pointerDrag;
        if (!draggedObj)
            return;

        var playerCard = draggedObj.GetComponent<PlayerCard>();

        if (!playerCard)
            return;

        var valid = IsValidDropForPlayer();
        if (!valid)
            return;

        // Execute Top Card
        var topCard = cards.LastOrDefault();
        if (!topCard || topCard == null)
            return;

        var (playerCanEnter, destroyCard) = topCard.ExecuteCardAction();
        var dndScript = draggedObj.GetComponent<DragDrop>();

        if (destroyCard)
        {
            cards.Remove(topCard);
            topCard.DestroyCard();
        }

        if (playerCanEnter)
        {
            var pos = GetTopCardPosition();
            dndScript.SetValidDrop(x, y, pos);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetPreview(eventData.pointerDrag, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetPreview(eventData.pointerDrag, false);
    }

    private void SetPreview(GameObject draggedObj, bool value)
    {
        if (!IsValidDropForPlayer())
            return;

        if (!draggedObj)
            return;

        var playerCard = draggedObj.GetComponent<PlayerCard>();
        if (!playerCard)
            return;

        var topCard = cards.LastOrDefault();

        if (!topCard)
            return;

        if (value)
        {
            var (previewCardStats, previewPlayerStats) = topCard.GetPreviewStats();
            topCard.ShowPreview(previewCardStats);
            playerCard.ShowPreview(previewPlayerStats);
        }
        else
        {
            topCard.ResetPreview();
            playerCard.ResetPreview();
        }

        Debug.Log("set preview: " + topCard + ", " + value);
    }


    public void ResetDropValidity(bool instant = false)
    {
        SetNotAvailableHover(true, instant);
    }

    public void UpdateDropValidity(bool instant = false)
    {
        var valid = IsValidDropForPlayer();
        SetNotAvailableHover(valid, instant);
    }

    public bool IsValidDropForPlayer()
    {
        var p = GameController.Instance.playerCard;
        var distance = Mathf.Abs(x - p.x) + Mathf.Abs(y - p.y);
        return distance == 1;
    }

    private void SetNotAvailableHover(bool valid, bool instant = false)
    {
        if (!notAvailableHover)
            return;

        notAvailableHover.DOFade(valid ? 0 : GameController.Instance.playGrid.notAvailableAlpha,
            instant ? 0 : fadeTime);
    }

    private Vector3 GetTopCardPosition()
    {
        return !cards.Any() ? transform.position : cards.Last().transform.position;
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