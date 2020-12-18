using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private float cardSpacingX = 2;
    [SerializeField] private float cardSpacingY = 3;

    [SerializeField] private int sizeX = 4;
    [SerializeField] private int sizeY = 3;

    [SerializeField] private int cardsPerDeck = 4;

    [SerializeField] private Deck[,] grid;

    [Header("Prefabs")] [SerializeField] private GameObject deckPrefab;

    void Awake()
    {
        grid = new Deck[sizeX, sizeY];
        SpawnDecks();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void SpawnDecks()
    {
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                var pos = transform.position - new Vector3(
                    x * cardSpacingX,
                    y * cardSpacingY,
                    0);
                grid[x, y] = SpawnDeck(x, y, pos);
            }
        }
    }

    private Deck SpawnDeck(int a, int b, Vector3 position)
    {
        var spawned = Instantiate(deckPrefab, transform, false);
        spawned.name = $"Deck {a + 1}-{b + 1}";
        spawned.transform.position = position;

        var deck = spawned.GetComponent<Deck>();
        deck.InitWithCards(cardsPerDeck, Random.value <= 0.5f);
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