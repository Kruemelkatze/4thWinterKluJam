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
    [SerializeField] [Min(0)] private int numberOfDoors = 4;

    [SerializeField] private bool seeOnlyEnvironment = true;
    public float notAvailableAlpha = 0.7f;

    [SerializeField] private Deck[,] grid;

    [Header("Prefabs")] [SerializeField] private GameObject deckPrefab;

    [SerializeField] private Cards.Data.CardList availableCards;

    public void Init()
    {
        grid = new Deck[sizeX, sizeY];
        SpawnDecks();
        ResetDeckDropValidites(true);
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

    public bool IsInBounds(int x, int y)
    {
        if (x >= grid.GetLength(0) || y >= grid.GetLength(1))
            return false;

        return grid[x, y];
    }

    public void UpdateDeckDropValidities(bool instant = false, bool noAudio = false)
    {
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                var deck = grid[x, y];
                deck.UpdateDropValidity(instant);
            }
        }

        if (!instant && !noAudio)
        {
            AudioController.Instance.PlayRandomSound("card_flip");
        }
    }

    public void ResetDeckDropValidites(bool instant = false, bool noAudio = true)
    {
        if (seeOnlyEnvironment)
        {
            UpdateDeckDropValidities(instant, noAudio);
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
        var xl = grid.GetLength(0);
        var yl = grid.GetLength(1);

        var doorPositions = new HashSet<(int x, int y)>();
        var tries = xl * yl;
        while (tries-- > 0 && doorPositions.Count < numberOfDoors)
        {
            var rx = Random.Range(0, xl);
            var ry = Random.Range(0, yl);

            doorPositions.Add((rx, ry));
        }


        for (var x = 0; x < xl; x++)
        {
            for (var y = 0; y < yl; y++)
            {
                var pos = GetPosition(x, y);
                var hasDoor = doorPositions.Contains((x, y));
                grid[x, y] = SpawnDeck(x, y, pos, hasDoor);
            }
        }
    }

    private Deck SpawnDeck(int x, int y, Vector3 position, bool hasDoor)
    {
        var spawned = Instantiate(deckPrefab, transform, false);
        spawned.name = $"Deck {y + 1}-{x + 1}";
        spawned.transform.position = position;

        var deck = spawned.GetComponent<Deck>();
        deck.x = x;
        deck.y = y;
        deck.InitWithCards(availableCards, cardsPerDeck, hasDoor);
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