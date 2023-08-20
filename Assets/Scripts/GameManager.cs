using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour {

    [SerializeField] private bool _isGameOver;

    void Update() {
        if (_isGameOver && Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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
