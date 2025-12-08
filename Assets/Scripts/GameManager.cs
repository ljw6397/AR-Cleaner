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

    [Header("Result UI")]
    public Text stageClearText;
    public Text stageFailText;
    public CanvasGroup clearGroup;
    public CanvasGroup failGroup;

    private float currentTime;
    private bool stageActive = false;
    private Coroutine timerFlashRoutine;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentStageIndex = PlayerPrefs.GetInt("SelectedStageIndex", 0);

        if (stageClearText != null) stageClearText.gameObject.SetActive(false);
        if (stageFailText != null) stageFailText.gameObject.SetActive(false);

    }
    void Update()
    {
        if (!stageActive) return;

        currentTime -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.CeilToInt(currentTime).ToString();

        if (currentTime <= 15f && timerFlashRoutine == null)
        {
            timerFlashRoutine = StartCoroutine(FlashTimer());
        }

        if (currentTime <= 0)
        {
            EndStage(false);
        }

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

        if (timerFlashRoutine != null)
        {
            StopCoroutine(timerFlashRoutine);
            timerFlashRoutine = null;
            timerText.color = Color.white; 
        }

        if (success)
        {
            PlayerPrefs.SetInt($"Stage{currentStageIndex + 1}_Clear", 1);

            stageClearText.gameObject.SetActive(true);
            StartCoroutine(FadeIn(clearGroup, 1f));

            StartCoroutine(LoadStageSelectAfterDelay());
        }
        else
        {
            stageFailText.gameObject.SetActive(true);
            StartCoroutine(FadeIn(failGroup, 1f));

            StartCoroutine(LoadStageSelectAfterDelay());
        }
    }

    private IEnumerator LoadStageSelectAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelect");
    }

    private IEnumerator FadeIn(CanvasGroup group, float duration)
    {
        float t = 0f;
        group.alpha = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        group.alpha = 1f;
    }

    private IEnumerator FlashTimer()
    {
        Color normalColor = timerText.color;
        Color flashColor = Color.red;

        float speed = 0.5f;

        while (true)
        {
            // 빨간색으로 변경
            timerText.color = flashColor;
            yield return new WaitForSeconds(speed);

            // 원래 색으로 변경
            timerText.color = normalColor;
            yield return new WaitForSeconds(speed);
        }
    }
}
