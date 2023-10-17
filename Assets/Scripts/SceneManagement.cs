using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public GameObject pausePanel;
    public static bool isPaused = false;

    private PlayerControls playerControls;

    private void Start()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    private void Update()
    {
        if (pausePanel != null && playerControls.Player.Pause.WasPerformedThisFrame())
        {
            bool pauseSwitch = !pausePanel.activeSelf;
            pausePanel.SetActive(pauseSwitch);
            isPaused = pauseSwitch;
        }
    }

    public void SetPause(bool val)
    {
        isPaused = val;
    }

    public void SetLevelToLoad(string fileName)
    {
        GameManager.levelToLoad = "MainLevels/" + fileName;
    }

    public void LoadMainMenu()
    {
        isPaused = false;
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene()
    {
        isPaused = false;
        GameManager.isPlayMode = true;
        SceneManager.LoadScene(1);
    }

    public void LoadLevelEditor()
    {
        isPaused = false;
        GameManager.isPlayMode = false;
        SceneManager.LoadScene(2);
    }

    public void RestartScene()
    {
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {

    }
}
