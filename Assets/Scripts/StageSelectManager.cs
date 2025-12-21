using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
   
    public Button[] stageButtons;

    void OnEnable()
    {
        RefreshStageButtons();
    }

    void Start()
    {
        SoundManager.Instance.PlayStageSelectBGM();
    }

    void RefreshStageButtons()
    {
        SetButtonState(stageButtons[0], true);

        for (int i = 1; i < stageButtons.Length; i++)
        {
            int prevStageClear = PlayerPrefs.GetInt($"Stage{i}_Clear", 0);

            bool isUnlocked = prevStageClear == 1;

            SetButtonState(stageButtons[i], isUnlocked);
        }
    }

    void SetButtonState(Button button, bool isUnlocked)
    {
        button.interactable = isUnlocked;

        Image img = button.GetComponent<Image>();

        if (isUnlocked)
        {
            img.color = Color.white;      
        }
        else
        {
            img.color = Color.red;        
        }
    }

    public void SelectStage(int stageIndex)
    {
        StartCoroutine(SelectStageRoutine(stageIndex));
    }

    IEnumerator SelectStageRoutine(int stageIndex)
    {
        SoundManager.Instance.PlayStageSelect();

        yield return new WaitForSeconds(0.3f);

        PlayerPrefs.SetInt("SelectedStageIndex", stageIndex);
        SceneManager.LoadScene("SampleScene");
    }
}
