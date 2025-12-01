using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Stage> stages = new List<Stage>();
    public int currentStageIndex = 0;

    [Header("UI")]
    public Text stageText;
    public Text timerText;
    public Text targetScoreText;

    private float currentTime;
    private bool stageActive = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartStage(currentStageIndex);
    }

    void Update()
    {
        if (!stageActive) return;

        currentTime -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.CeilToInt(currentTime).ToString();

        if (currentTime <= 0)
        {
            EndStage(false);
        }

        // 목표 점수 달성 체크
        if (ScoreManager.Instance.score >= stages[currentStageIndex].targetScore)
        {
            EndStage(true);
        }
    }

    public void StartStage(int index)
    {
        if (index >= stages.Count)
        {
            Debug.Log("모든 스테이지 클리어!");
            return;
        }

        Stage stage = stages[index];
        stageText.text = "Stage " + stage.stageNumber;
        targetScoreText.text = "목표 점수: " + stage.targetScore;
        currentTime = stage.timeLimit;

        ScoreManager.Instance.score = 0;
        ScoreManager.Instance.UpdateUI();

        stageActive = true;
    }

    void EndStage(bool success)
    {
        stageActive = false;
        if (success)
        {
            Debug.Log("Stage Clear!");
            currentStageIndex++;
            StartStage(currentStageIndex);
        }
        else
        {
            Debug.Log("Stage Failed...");
        }
    }
}
