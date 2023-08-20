using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour {

    [SerializeField] private bool _isGameOver;

    public void SetGameOver(bool isGameOver) {
        _isGameOver = isGameOver;
    }
    public void Update() {
        if (_isGameOver && Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
