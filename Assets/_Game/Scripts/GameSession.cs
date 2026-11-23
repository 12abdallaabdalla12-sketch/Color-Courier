using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private CourierPlayer player;
    [SerializeField] private TMP_Text hudText;
    [SerializeField, Min(1)] private int requiredCells = 3;
    [SerializeField] private int levelNumber = 1;
    [SerializeField] private string levelTitle = "Workshop";
    [SerializeField] private string nextLevelScene;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject replayButton;
    [SerializeField] private GameObject nextLevelButton;

    public int CollectedCells { get; private set; }
    public int RequiredCells => requiredCells;
    public bool ExitUnlocked => CollectedCells >= requiredCells;
    public bool IsComplete { get; private set; }
    public bool IsPaused { get; private set; }

    private CourierColor displayedColor;

    private void Start()
    {
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        RefreshHUD();
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.rKey.wasPressedThisFrame)
            {
                RestartLevel();
                return;
            }

            if (keyboard.escapeKey.wasPressedThisFrame && !IsComplete)
                SetPaused(!IsPaused);
        }

        if (!IsComplete && !IsPaused && player != null &&
            player.CurrentColor != displayedColor)
            RefreshHUD();
    }

    public void CollectCell()
    {
        if (ExitUnlocked || IsComplete || IsPaused) return;
        CollectedCells++;
        RefreshHUD();
    }

    public void CompleteLevel()
    {
        if (!ExitUnlocked || IsComplete || IsPaused) return;

        IsComplete = true;
        if (player != null) player.enabled = false;
        RefreshHUD();

        bool hasNextLevel = !string.IsNullOrEmpty(nextLevelScene);
        if (resultText != null)
            resultText.text = hasNextLevel
                ? "LEVEL " + levelNumber + " COMPLETE\nAll " + requiredCells + " energy cells delivered."
                : "ALL DELIVERIES COMPLETE!\nYou finished the final level.";

        if (nextLevelButton != null) nextLevelButton.SetActive(hasNextLevel);
        if (winPanel != null) winPanel.SetActive(true);
        SelectButton(hasNextLevel ? nextLevelButton : replayButton);
    }

    public void ResumeGame()
    {
        if (!IsComplete) SetPaused(false);
    }

    private void SetPaused(bool paused)
    {
        IsPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
        if (player != null) player.enabled = !paused;
        if (pausePanel != null) pausePanel.SetActive(paused);
        SelectButton(paused ? resumeButton : null);
    }

    public void RestartLevel()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelScene)) LoadScene(nextLevelScene);
    }

    public void ReturnToMenu()
    {
        LoadScene("MainMenu");
    }

    private void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    private void SelectButton(GameObject button)
    {
        if (EventSystem.current == null) return;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }

    private void RefreshHUD()
    {
        if (player != null) displayedColor = player.CurrentColor;
        if (hudText == null) return;

        string objective = IsComplete ? "Delivery complete!"
            : ExitUnlocked ? "Exit unlocked! Return to the exit pad."
            : "Collect every energy cell, then reach the exit.";

        hudText.text = "LEVEL " + levelNumber + "  |  " + levelTitle.ToUpper() +
            "\nCells: " + CollectedCells + " / " + requiredCells +
            "    Color: " + displayedColor.ToString().ToUpper() +
            "\nWASD: Move   1: Red   2: Blue   3: Yellow" +
            "\nR: Restart   Esc: Pause\n" + objective;
    }
}
