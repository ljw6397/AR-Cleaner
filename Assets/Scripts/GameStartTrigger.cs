using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartTrigger : MonoBehaviour
{
    void Start()
    {
        int idx = PlayerPrefs.GetInt("SelectedStageIndex", 0);
        GameManager.Instance.StartStage(idx);
    }
}
