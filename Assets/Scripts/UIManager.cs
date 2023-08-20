using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {
    [SerializeField] private TMP_Text _scoreNumberText;

    public void UpdateScoreNumberText(int newScore) {
        _scoreNumberText.SetText(newScore.ToString());
    } 
}
