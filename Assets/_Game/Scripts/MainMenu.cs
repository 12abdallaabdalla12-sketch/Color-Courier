using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject firstLevelButton;

    private void Start()
    {
        Time.timeScale = 1f;
        ShowTitle();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame &&
            levelPanel != null && levelPanel.activeSelf)
            ShowTitle();
    }

    public void ShowLevels()
    {
        titlePanel.SetActive(false);
        levelPanel.SetActive(true);
        SelectButton(firstLevelButton);
    }

    public void ShowTitle()
    {
        titlePanel.SetActive(true);
        levelPanel.SetActive(false);
        SelectButton(playButton);
    }

    public void LoadLevel(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SelectButton(GameObject button)
    {
        if (EventSystem.current == null) return;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }
}
