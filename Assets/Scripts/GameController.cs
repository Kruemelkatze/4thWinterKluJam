using Cards;
using UnityEditor;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [Header("Game States")] [SerializeField]
    private bool isPaused;

    [Header("UI")] [SerializeField] private GameObject gameUi;
    [SerializeField] private GameObject pauseUi;

    public Grid playGrid;
    public PlayerCard playerCard;

    private void Start()
    {
        if (!AudioController.Instance.IsMusicPlaying)
        {
            AudioController.Instance.PlayDefaultMusic();
        }

        SetPause(false);
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

    //  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ PRIVATE  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    private void SetPause(bool paused)
    {
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
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameController))]
public class GameControlTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var gct = target as GameController;

        if (GUILayout.Button("Restart"))
        {
            SceneController.Instance.RestartScene();
        }
    }
}
#endif