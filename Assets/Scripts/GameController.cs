using System.Collections;
using Cards;
using UnityEditor;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField] private Narrator.Narrator narrator;

    [Header("Game States")] [SerializeField]
    public int heroPoints = 0;

    [SerializeField] private int level = 0;
    [SerializeField] private GameState gameState = GameState.Starting;
    [SerializeField] private bool isPaused;

    public Grid playGrid;

    [Header("UI")] [SerializeField] private GameObject gameUi;
    [SerializeField] private GameObject pauseUi;

    [Header("Prefabs and Stuff")] public PlayerCard playerCard;
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
        if (!AudioController.Instance.IsMusicPlaying)
        {
            AudioController.Instance.PlayDefaultMusic();
        }

        gameState = GameState.Starting;
        if (!playGrid)
        {
            ChangeLevel();
        }

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
    }

    public void DoorReached(int x, int y)
    {
        Debug.Log($"Door reached: {x},{y}");
        ChangeLevel();
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

        level++;
        var newGrid = GetGrid(level);
        if (playGrid)
        {
            Destroy(playGrid.gameObject);
        }

        playGrid = newGrid;

        yield return new WaitForSeconds(0.5f);

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
}
#endif