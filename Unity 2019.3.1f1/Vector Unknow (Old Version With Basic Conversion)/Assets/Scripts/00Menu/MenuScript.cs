using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public Texture2D cursorArrow;

    public void StartGame()
    {
        SceneManager.LoadScene("02Puzzle1", LoadSceneMode.Single);
    }

    public void StartTutorial()
    {
        Debug.Log("Tutorial");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
