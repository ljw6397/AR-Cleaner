using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;
    public Text scoreText; // UI Text 연결!

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "점수 : " + score;
    }
}
