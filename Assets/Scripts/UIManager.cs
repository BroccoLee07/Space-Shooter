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
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private float _gameOverFlickerIntervalTime = 1f;
    [SerializeField] private TMP_Text _restartText;

    public void SetScoreNumberText(int newScore) {
        _scoreNumberText.SetText(newScore.ToString());
    }

    public void SetHPBarSprite(int currentHP) {
        _hpBar.sprite = _hpBarSprites[currentHP];
    }

    public void DisplayGameOverText(bool isVisible) {
        _gameOverText.enabled = isVisible;
        _restartText.enabled = isVisible;
        if (isVisible) { 
            StartCoroutine(FlickerGameOverTextCoroutine());
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
