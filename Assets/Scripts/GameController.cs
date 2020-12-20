using System;
using System.Collections;
using Cards;
using Extensions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    [Header("Game States")] [SerializeField]
    public int heroPoints = 0;

    [SerializeField] private int level = 0;
    [SerializeField] private int maxLevel = 5;

    [SerializeField] private GameState gameState = GameState.Starting;
    [SerializeField] private bool isPaused;

    public Grid playGrid;

    [Header("UI")] [SerializeField] private GameObject gameUi;
    [SerializeField] private GameObject pauseUi;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI gloryText;

    [SerializeField] private TextMeshProUGUI ventureForthText;
    [SerializeField] private TextMeshProUGUI diedText;
    [SerializeField] private TextMeshProUGUI finishedText;

    [SerializeField] private GameObject restartButton;
    [SerializeField] private Image fadeImage;


    [Header("Prefabs and Stuff")] public PlayerCard playerCard;
    public Narrator.Narrator narrator;

    public GameObject[] levels;
    public GameObject gridPrefab;

    public GameState GameState => gameState;

    private GameState _prePauseState = GameState.Starting;

    private void Awake()
    {
        if (!ThisIsTheSingletonInstance())
        {
            return;
        }
    }

    private void Start()
    {
        AudioController.Instance.PlayDefaultMusic();

        gameState = GameState.Starting;
        if (!playGrid)
        {
            ChangeLevel();
        }

        UpdateUI();

        _prePauseState = gameState;
        SetPause(false);
        gameState = GameState.Playing;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPause(!isPaused);
        }

        if (isPaused)
        {
            return;
        }
    }

    //  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ PUBLIC  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public void PauseGame() => SetPause(true);
    public void ContinueGame() => SetPause(false);

    public void PlayerHealthReachedZero()
    {
        gameState = GameState.PlayerDied;
        Debug.Log(nameof(PlayerHealthReachedZero));
        UpdateUI();
    }

    public void DoorReached(int x, int y)
    {
        Debug.Log($"Door reached: {x},{y}");
        ChangeLevel();
    }

    public void AddHeroPoints(int diff)
    {
        heroPoints += diff;
        if (gloryText)
        {
            gloryText.SetText(heroPoints.ToString());
        }
    }

    public void Finished()
    {
        gameState = GameState.Finished;
        Debug.Log("Finished");
        UpdateUI();
    }


    //  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ PRIVATE  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    private void SetPause(bool paused)
    {
        if (paused)
        {
            _prePauseState = gameState;
            gameState = GameState.Paused;
        }
        else
        {
            gameState = _prePauseState;
        }

        isPaused = paused;

        if (gameUi != null)
        {
            gameUi.SetActive(!paused);
        }

        if (pauseUi != null)
        {
            pauseUi.SetActive(paused);
        }

        // Stopping time depends on your game! Turn-based games maybe don't need this
        Time.timeScale = paused ? 0 : 1;

        // Whatever else there is to do...
        // Deactivate other UI, etc.
    }

    private void ChangeLevel()
    {
        StartCoroutine(ChangeLevelCoroutine());
    }

    private IEnumerator ChangeLevelCoroutine()
    {
        gameState = GameState.Changing;

        if (level == 0)
        {
            AudioController.Instance.PlayRandomSound("door");
        }

        var newGrid = GetGrid(level);
        if (playGrid)
        {
            Destroy(playGrid.gameObject);
        }

        if (level < maxLevel)
        {
            if (level != 0)
            {
                fadeImage.Fade(true);
            }

            level++;
            UpdateUI();

            playGrid = newGrid;

            yield return new WaitForSeconds(0.5f);

            if (level != 1)
            {
                fadeImage.Fade(false);
            }

            if (level == 1 || !playGrid.IsInBounds(playerCard.x, playerCard.y))
            {
                var (pos, x, y) = playGrid.GetPlayerSpawnPosition();
                playerCard.SetPosition(x, y, pos);
            }
            else
            {
                var pos = playGrid.GetPosition(playerCard.x, playerCard.y);
                playerCard.SetPosition(pos);
            }

            playerCard.Show(true);

            playGrid.ResetDeckDropValidites();
            gameState = GameState.Playing;
            UpdateUI();
        }
        else
        {
            Finished();
        }
    }

    private void UpdateUI()
    {
        levelText.SetText($"{level} / {maxLevel}");
        gloryText.SetText(heroPoints.ToString());

        switch (GameState)
        {
            case GameState.PlayerDied:
                ventureForthText.gameObject.SetActive(false);
                diedText.gameObject.SetActive(true);
                finishedText.gameObject.SetActive(false);
                restartButton.SetActive(true);
                break;
            case GameState.Finished:
                ventureForthText.gameObject.SetActive(false);
                diedText.gameObject.SetActive(false);
                finishedText.gameObject.SetActive(true);
                restartButton.SetActive(true);
                break;
            case GameState.Starting:
            case GameState.Playing:
            case GameState.Paused:
            case GameState.Changing:
            default:
                ventureForthText.gameObject.SetActive(true);
                diedText.gameObject.SetActive(false);
                finishedText.gameObject.SetActive(false);
                restartButton.SetActive(false);
                break;
        }
    }

    private Grid GetGrid(int forLevel)
    {
        GameObject prefabForGrid;
        if (levels != null && forLevel < levels.Length)
        {
            prefabForGrid = levels[forLevel];
        }
        else
        {
            prefabForGrid = gridPrefab;
        }

        var gridObj = Instantiate(prefabForGrid);
        var grid = gridObj.GetComponent<Grid>();

        grid.Init();

        if (!grid)
        {
            Destroy(gridObj);
        }

        return grid;
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(GameController))]
    public class GameControlTestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var gct = target as GameController;

            if (gct == null)
                return;

            if (!Application.isPlaying)
                return;

            if (GUILayout.Button("Restart"))
            {
                SceneController.Instance.RestartScene();
            }

            if (GUILayout.Button("Next Level"))
            {
                gct.ChangeLevel();
            }
        }
    }
#endif
}