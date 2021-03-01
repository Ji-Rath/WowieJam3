using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject MenuCanvas;
    public GameObject HelpCanvas;
    public int mainGameSceneIndex;

    public void StartGame()
    {
        SceneManager.LoadScene(mainGameSceneIndex);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MenuCanvas.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void GotoHelp()
    {
        MenuCanvas.SetActive(false);
        HelpCanvas.SetActive(true);
    }

    public void GotoMenu()
    {
        MenuCanvas.SetActive(true);
        HelpCanvas.SetActive(false);
    }
}
