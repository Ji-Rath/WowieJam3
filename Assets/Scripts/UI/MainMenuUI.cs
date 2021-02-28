using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject MenuCanvas;
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
}
