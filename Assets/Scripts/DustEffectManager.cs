using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustEffectManager : MonoBehaviour
{
    public static DustEffectManager Instance;

    [Header("Dust Effects")]
    public GameObject bigDustEffect;
    public GameObject smallDustEffect;
    public GameObject cleanFinishEffect;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayBigDust(Vector3 pos)
    {
        Spawn(bigDustEffect, pos, 0.05f);   
    }

    public void PlaySmallDust(Vector3 pos)
    {
        Spawn(smallDustEffect, pos, 0.03f); 
    }

    public void PlayCleanFinish(Vector3 pos)
    {
        Spawn(cleanFinishEffect, pos, 0.05f);
    }

    void Spawn(GameObject prefab, Vector3 pos, float scale)
    {
        if (prefab == null) return;

        GameObject fx = Instantiate(prefab, pos, Quaternion.identity);
        fx.transform.localScale = Vector3.one * scale;

        Destroy(fx, 2f);
    }
}
