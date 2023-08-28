using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour {

    public enum GameState { 
        MAIN_MENU,
        GAME_START,
        GAME_OVER
    }
    // TODO: Replace with GameState
    [SerializeField] private bool _isGameOver;
    // State where game is started and waiting for user's input (enter key)
    [SerializeField] private bool _isGameStart;

    void Start() {
        _isGameStart = false;
    }

    void Update() {
        if (_isGameOver && Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public bool GetGameStart() {
        return _isGameStart;
    }
    public void SetGameStart(bool isGameStart) {
        _isGameStart = isGameStart;
    }

    public void SetGameOver(bool isGameOver) {
        _isGameOver = isGameOver;
    }

    public void StartGame() {
        _isGameOver = false;
        LoadScene("Game");
    }

    public void DisplayMainMenu() {
        // TODO: Create Config/Constants file that contains necessary hardcoded values such as scene names, etc
        LoadScene("MainMenu");
    }

    private void LoadScene(string sceneName) { 
        // Using GetSceneName to make sure scene of specific name exists and using that name
        if (SceneManager.GetSceneByName(sceneName) != null) {
            if (SceneManager.GetSceneByName(sceneName).isLoaded) {
                ActivateSceneGameObjects(sceneName);
            } else {
                DeactivateSceneGameObjects(SceneManager.GetActiveScene().name);
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);  
            }         
        }
    }

    private void ActivateSceneGameObjects(string sceneName) {
        Scene newScene = SceneManager.GetSceneByName(sceneName);
        if (newScene.IsValid()) { 
            GameObject[] rootObjects = newScene.GetRootGameObjects();
            foreach (GameObject rootObject in rootObjects) {
                rootObject.SetActive(true);
            }
        }
    }

    private void DeactivateSceneGameObjects(string sceneName) {
        Scene oldScene = SceneManager.GetSceneByName(sceneName);
        if (oldScene.IsValid()) {
            GameObject[] rootObjects = oldScene.GetRootGameObjects();
            foreach (GameObject rootObject in rootObjects) {
                rootObject.SetActive(false);
            }
        }
    }
}
