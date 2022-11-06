using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject endMenu;
    [SerializeField] private GameObject game;

    public void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
        }
    }

    public void StartGame() {
        mainMenu.SetActive(false);
        game.SetActive(true);
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void EndGame() {
        game.SetActive(false);
        endMenu.SetActive(true);
    }

    public void NewGame() {
        SceneManager.LoadScene("MainScene");
    }

    public void PauseGame() {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

}
