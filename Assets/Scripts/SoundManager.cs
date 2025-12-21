using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Dust Sounds")]
    public AudioClip squeakClip;
    public AudioClip cleanFinishClip;

    [Header("Stage Sounds")]
    public AudioClip stageClearClip;
    public AudioClip stageFailClip;

    [Header("BGM Clips")]
    public AudioClip stageSelectBGM;
    public AudioClip ingameBGM;

    [Header("UI Sounds")]
    public AudioClip stageSelectClip;

    [Header("Timer Sound")]
    public AudioClip timerTickClip;
    [Range(0f, 1f)]
    public float timerTickVolume = 0.4f;

    [Header("Skill Sounds")]
    public AudioClip skillUseClip;
    public AudioClip skillReadyClip;   
    [Range(0f, 1f)] public float skillUseVolume = 0.6f;
    [Range(0f, 1f)] public float skillReadyVolume = 0.6f;

    public AudioSource timerSource;

    [Header("Volume")]
    [Range(0f, 1f)] public float uiVolume = 0.5f;
    [Range(0f, 1f)] public float squeakVolume = 0.3f;
    [Range(0f, 1f)] public float finishVolume = 0.7f;
    [Range(0f, 1f)] public float bgmVolume = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            AudioSource[] sources = GetComponents<AudioSource>();
            bgmSource = sources[0];
            sfxSource = sources[1];
            timerSource = sources[2];   
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ---------- BGM ----------
    public void PlayIngameBGM()
    {
        if (bgmSource.clip == ingameBGM && bgmSource.isPlaying)
            return;

        bgmSource.Stop();
        bgmSource.clip = ingameBGM;
        bgmSource.volume = bgmVolume;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlayStageSelectBGM()
    {
        if (bgmSource.clip == stageSelectBGM && bgmSource.isPlaying)
            return;

        bgmSource.Stop();
        bgmSource.clip = stageSelectBGM;
        bgmSource.volume = bgmVolume;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    // ---------- SFX ----------
    public void PlaySqueak()
    {
        sfxSource.pitch = Random.Range(0.9f, 1.1f);
        sfxSource.PlayOneShot(squeakClip, squeakVolume);
    }

    public void PlayCleanFinish()
    {
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(cleanFinishClip, finishVolume);
    }

    public void PlayStageClear()
    {
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(stageClearClip, 1f);
    }

    public void PlayStageFail()
    {
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(stageFailClip, 1f);
    }

    public void PlayStageSelect()
    {
        sfxSource.pitch = Random.Range(0.95f, 1.05f);
        sfxSource.PlayOneShot(stageSelectClip, uiVolume);
    }

    public void PlayTimerTick()
    {
        if (timerSource.isPlaying) return;

        timerSource.clip = timerTickClip;
        timerSource.volume = timerTickVolume;
        timerSource.loop = true;
        timerSource.Play();
    }

    public void StopTimerTick()
    {
        timerSource.Stop();
    }

    public void PlaySkillUse()
    {
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(skillUseClip, skillUseVolume);
    }

    public void PlaySkillReady()
    {
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(skillReadyClip, skillReadyVolume);
    }
}
