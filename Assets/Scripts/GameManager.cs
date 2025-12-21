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
    public Text targetScoreText;
    [Header("Countdown UI")]
    public Text countdownText;

    [Header("Timer UI")]
    public Image timerFillImage;

    [Header("Result UI")]
    public GameObject stageClearUI;
    public GameObject stageFailUI;
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

        if (stageClearUI != null) stageClearUI.SetActive(false);
        if (stageFailUI != null) stageFailUI.SetActive(false);

        StartStage(currentStageIndex);
    }

    void Update()
    {
        if (!stageActive) return;

        currentTime -= Time.deltaTime;

        // 게이지 비율 계산
        float ratio = currentTime / stages[currentStageIndex].timeLimit;
        timerFillImage.fillAmount = Mathf.Clamp01(ratio);

        if (currentTime <= 15f && timerFlashRoutine == null)
        {
            timerFlashRoutine = StartCoroutine(FlashTimer());
        }

        if (currentTime <= 15f)
        {
            SoundManager.Instance.timerSource.pitch = 1.4f;
        }
        else
        {
            SoundManager.Instance.timerSource.pitch = 1f;
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

        currentStageIndex = index;   // ← 이거 꼭 필요함

        Stage stage = stages[index];
        stageText.text = "Stage " + stage.stageNumber;
        targetScoreText.text = "목표 점수: " + stage.targetScore + " / ";

        currentTime = stage.timeLimit;

        timerFillImage.fillAmount = 1f;
        timerFillImage.color = Color.yellow;

        if (timerFlashRoutine != null)
        {
            StopCoroutine(timerFlashRoutine);
            timerFlashRoutine = null;
        }

        ScoreManager.Instance.score = 0;
        ScoreManager.Instance.UpdateUI();

        stageActive = false;

        StartCoroutine(StageCountdown());
    }

    private IEnumerator StageCountdown()
    {
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            countdownText.transform.localScale = Vector3.one * 1.2f;

            float t = 0f;
            while (t < 0.2f)
            {
                t += Time.deltaTime;
                countdownText.transform.localScale = Vector3.Lerp(
                    Vector3.one * 1.2f,
                    Vector3.one,
                    t / 0.2f
                );
                yield return null;
            }

            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);

        countdownText.gameObject.SetActive(false);

        stageActive = true;

        SoundManager.Instance.PlayIngameBGM();
        SoundManager.Instance.PlayTimerTick();   
    }

    void EndStage(bool success)
    {
        if (!stageActive) return;  
        stageActive = false;

        if (timerFlashRoutine != null)
        {
            StopCoroutine(timerFlashRoutine);
            timerFlashRoutine = null;
        }

        SoundManager.Instance.StopTimerTick();
        SoundManager.Instance.StopBGM();

        if (success)
        {
            SoundManager.Instance.PlayStageClear();

            PlayerPrefs.SetInt($"Stage{currentStageIndex + 1}_Clear", 1);

            stageClearUI.SetActive(true);
            clearGroup.alpha = 0f;
            StartCoroutine(FadeIn(clearGroup, 1f));
        }
        else
        {
            SoundManager.Instance.PlayStageFail();

            stageFailUI.SetActive(true);
            failGroup.alpha = 0f;
            StartCoroutine(FadeIn(failGroup, 1f));
        }

        StartCoroutine(LoadStageSelectAfterDelay());
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
        Transform tr = group.transform;

        Vector3 startScale = Vector3.one * 0.8f;
        Vector3 endScale = Vector3.one;

        tr.localScale = startScale;

        while (t < duration)
        {
            t += Time.deltaTime;
            float ratio = t / duration;

            group.alpha = Mathf.Lerp(0f, 1f, ratio);
            tr.localScale = Vector3.Lerp(startScale, endScale, ratio);

            yield return null;
        }

        group.alpha = 1f;
        tr.localScale = endScale;
    }

    private IEnumerator FlashTimer()
    {
        Color normalColor = timerFillImage.color;
        Color flashColor = Color.red;
        float speed = 0.5f;

        while (true)
        {
            timerFillImage.color = flashColor;
            yield return new WaitForSeconds(speed);

            timerFillImage.color = normalColor;
            yield return new WaitForSeconds(speed);
        }
    }
}
