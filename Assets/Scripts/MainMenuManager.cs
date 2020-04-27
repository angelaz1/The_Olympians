using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void startTutorial() {
        SceneManager.LoadScene("Tutorial");
    }

    public void exitGame() {
        Application.Quit();
    }
}
