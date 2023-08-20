using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] private TMP_Text _scoreNumberText;

    [SerializeField] private Image _hpBar;
    [SerializeField] private Sprite[] _hpBarSprites;
    
    public void SetScoreNumberText(int newScore) {
        _scoreNumberText.SetText(newScore.ToString());
    }

    public void SetHPBarSprite(int currentHP) {
        _hpBar.sprite = _hpBarSprites[currentHP];
    }
}
