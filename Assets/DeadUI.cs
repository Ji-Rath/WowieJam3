using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeadUI : MonoBehaviour
{
    public TMP_Text FinalScoreText;
    public GameObject DeadUICanvas;
    public int mainGameSceneIndex = 0;
    public int mainMenuSceneIndex = 1;

    public void DisplayScore(int Points)
    {
        FinalScoreText.text = "Final Score: " + Points;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(mainGameSceneIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
}
