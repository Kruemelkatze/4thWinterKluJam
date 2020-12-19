using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    [SerializeField] private float cardSpacingX = 2;
    [SerializeField] private float cardSpacingY = 2.5f;

    [SerializeField] private int sizeX = 4;
    [SerializeField] private int sizeY = 3;

    [SerializeField] private int playerSpawnX = -1;
    [SerializeField] private int playerSpawnY = 1;

    [SerializeField] private int cardsPerDeck = 4;

    [SerializeField] private bool seeOnlyEnvironment = true;
    public float notAvailableAlpha = 0.7f;

    [SerializeField] private Deck[,] grid;

    [Header("Prefabs")] [SerializeField] private GameObject deckPrefab;

    [SerializeField] private Cards.Data.CardList availableCards;


    void Awake()
    {
        grid = new Deck[sizeX, sizeY];
        SpawnDecks();
    }

    private void Start()
    {
        ResetDeckDropValidites(true);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public (Vector3 pos, int x, int y) GetPlayerSpawnPosition()
    {
        var pos = GetPosition(playerSpawnX, playerSpawnY);
        return (pos, playerSpawnX, playerSpawnY);
    }

    public Vector3 GetPosition(int x, int y)
    {
        var pos = transform.position + new Vector3(
            (x - (sizeX - 1) / 2f) * cardSpacingX,
            ((sizeY - 1) / 2f - y) * cardSpacingY,
            0);

        return pos;
    }

    public void UpdateDeckDropValidities(bool instant = false)
    {
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                var deck = grid[x, y];
                deck.UpdateDropValidity(instant);
            }
        }
    }

    public void ResetDeckDropValidites(bool instant = false)
    {
        if (seeOnlyEnvironment)
        {
            UpdateDeckDropValidities(instant);
            return;
        }

        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                var deck = grid[x, y];
                deck.ResetDropValidity(instant);
            }
        }
    }

    private void SpawnDecks()
    {
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                var pos = GetPosition(x, y);
                grid[x, y] = SpawnDeck(x, y, pos);
            }
        }
    }

    private Deck SpawnDeck(int x, int y, Vector3 position)
    {
        var spawned = Instantiate(deckPrefab, transform, false);
        spawned.name = $"Deck {y + 1}-{x + 1}";
        spawned.transform.position = position;

        var deck = spawned.GetComponent<Deck>();
        deck.x = x;
        deck.y = y;
        deck.InitWithCards(availableCards, cardsPerDeck, Random.value <= 0.5f);
        return deck;
    }

    private void ReshuffleDecks()
    {
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                var deck = grid[x, y];
                if (deck)
                {
                    Destroy(deck.gameObject);
                    grid[x, y] = null;
                }
            }
        }

        grid = new Deck[sizeX, sizeY];
        SpawnDecks();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Grid))]
    private class GridEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!(target is Grid script))
                return;


            if (Application.isPlaying && GUILayout.Button("ReshuffleDecks"))
            {
                script.ReshuffleDecks();
            }
        }
    }
#endif
}