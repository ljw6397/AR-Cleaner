using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Dust Sounds")]
    public AudioClip squeakClip;      // »Çµæ»Çµæ
    public AudioClip cleanFinishClip; // Ã»¼Ò ³¡

    [Header("UI Sounds")]
    public AudioClip stageSelectClip;
    [Range(0f, 1f)]
    public float uiVolume = 0.5f;

    [Range(0f, 1f)]
    public float squeakVolume = 0.3f;

    [Range(0f, 1f)]
    public float finishVolume = 0.7f;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySqueak()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(squeakClip, squeakVolume);
    }

    public void PlayCleanFinish()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(cleanFinishClip, finishVolume);
    }

    public void PlayStageSelect()
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(stageSelectClip, uiVolume);
    }
}
