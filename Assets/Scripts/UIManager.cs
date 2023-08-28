using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [Header("Top UI")]
    [SerializeField] private TMP_Text _scoreNumberText;
    [SerializeField] private Image _hpBar;
    [SerializeField] private Sprite[] _hpBarSprites;
    [Space(10)]

    [Header("Middle UI")]
    [SerializeField] private TMP_Text _gameStartText;
    [SerializeField] private float _gameStartFlickerIntervalTime = 0.5f;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private float _gameOverFlickerIntervalTime = 0.5f;
    [SerializeField] private TMP_Text _restartText;

    public void SetScoreNumberText(int newScore) {
        _scoreNumberText.SetText(newScore.ToString());
    }

    public void SetHPBarSprite(int currentHP) {
        _hpBar.sprite = _hpBarSprites[currentHP];
    }

    public void DisplayGameStartText(bool isVisible) {
        Debug.Log($"Game start text should be displayed? {isVisible}");
        _gameStartText.enabled = isVisible;
        if (isVisible) { 
            StartCoroutine(FlickerGameStartTextCoroutine());
        // } else {
        //     StopCoroutine(FlickerGameStartTextCoroutine());
        }
    }

    public void DisplayGameOverText(bool isVisible) {
        _gameOverText.enabled = isVisible;
        _restartText.enabled = isVisible;
        if (isVisible) { 
            StartCoroutine(FlickerGameOverTextCoroutine());
        // } else {
        //     StopCoroutine(FlickerGameOverTextCoroutine());
        }
    }

    public IEnumerator FlickerGameStartTextCoroutine() {
        while (true) { 
            yield return new WaitForSeconds(_gameStartFlickerIntervalTime);
            if (_gameStartText.IsActive()) { 
                _gameStartText.enabled = false;
            } else {
                _gameStartText.enabled = true;
            }            
        }        
    }

    public IEnumerator FlickerGameOverTextCoroutine() {
        while (true) { 
            yield return new WaitForSeconds(_gameOverFlickerIntervalTime);
            if (_gameOverText.IsActive()) { 
                _gameOverText.enabled = false;
            } else {
                _gameOverText.enabled = true;
            }            
        }        
    }
}
