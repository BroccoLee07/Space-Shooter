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
    [SerializeField] private GameObject _gameOverText;
    
    public void SetScoreNumberText(int newScore) {
        _scoreNumberText.SetText(newScore.ToString());
    }

    public void SetHPBarSprite(int currentHP) {
        _hpBar.sprite = _hpBarSprites[currentHP];
    }

    public void DisplayGameOverText(bool isVisible) {
        _gameOverText.SetActive(isVisible);
    }
}
