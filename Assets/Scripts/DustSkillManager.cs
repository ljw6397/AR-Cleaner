using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DustSkillManager : MonoBehaviour
{
    public static DustSkillManager Instance;

    [Header("Skill Settings")]
    public float skillDuration = 5f;
    public float cooldown = 10f;

    [Header("UI")]
    public Image cooldownFill;
    public Image skillButtonImage;   

    [Header("Button Colors")]
    public Color normalColor = Color.white;
    public Color cooldownColor = new Color(0.6f, 0.6f, 0.6f, 1f);

    public bool IsSkillActive { get; private set; }
    private bool isCoolingDown;

    private void Awake()
    {
        Instance = this;
        cooldownFill.fillAmount = 0f;
    }

    public void OnSkillButtonClick()
    {
        if (isCoolingDown || IsSkillActive) return;

        SoundManager.Instance.PlaySkillUse(); 

        skillButtonImage.color = cooldownColor;

        StartCoroutine(SkillRoutine());
        StartCoroutine(CooldownRoutine());
    }

    IEnumerator SkillRoutine()
    {
        IsSkillActive = true;

        yield return new WaitForSeconds(skillDuration);

        IsSkillActive = false;
    }

    IEnumerator CooldownRoutine()
    {
        isCoolingDown = true;
        float t = cooldown;

        while (t > 0)
        {
            t -= Time.deltaTime;
            cooldownFill.fillAmount = 1f - (t / cooldown);
            yield return null;
        }

        cooldownFill.fillAmount = 1f;

        isCoolingDown = false;
        skillButtonImage.color = normalColor;

        SoundManager.Instance.PlaySkillReady();
    }
}
