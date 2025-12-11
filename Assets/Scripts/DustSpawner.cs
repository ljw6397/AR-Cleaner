using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DustSpawner : MonoBehaviour
{
    [Header("AR Components")]
    public ARPlaneManager planeManager;

    [Header("Spawn Settings")]
    [Range(0f, 1f)]
    [Tooltip("평면 감지 시 먼지가 생성될 확률 (0~1)")]
    public float spawnChance = 0.5f;

    [Tooltip("한 평면당 생성할 먼지의 최소 개수")]
    public int minDustCount = 2;

    [Tooltip("한 평면당 생성할 먼지의 최대 개수")]
    public int maxDustCount = 6;

    [Tooltip("먼지 생성 시 평면 중심으로부터의 최대 거리")]
    public float spawnRadius = 0.5f;


    [Header("Dust Prefabs & Scores")]
    public List<GameObject> dustPrefabs = new List<GameObject>();
    public List<int> dustScores = new List<int>(); // prefab과 같은 순서로 점수

    [Header("Dust Lifetime & Respawn")]
    public float dustLifetime = 5f; // 먼지 지속 시간
    public float respawnDelay = 2f; // 삭제 후 재생성 대기 시간

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
        {
            StartCoroutine(SpawnDustLoop(plane));
        }
    }

    private IEnumerator SpawnDustLoop(ARPlane plane)
    {
        while (plane != null && plane.gameObject.activeInHierarchy)
        {
            if (dustPrefabs.Count == 0 || dustScores.Count != dustPrefabs.Count)
                yield break;

            if (Random.value < spawnChance)
            {
                int dustCount = Random.Range(minDustCount, maxDustCount + 1);
                List<GameObject> spawnedDusts = new List<GameObject>();

                for (int i = 0; i < dustCount; i++)
                {
                    int randomIndex = Random.Range(0, dustPrefabs.Count);
                    GameObject randomDust = dustPrefabs[randomIndex];
                    int dustScore = dustScores[randomIndex];

                    Vector2 randomPos2D = Random.insideUnitCircle * spawnRadius;
                    Vector3 spawnPos = plane.center + plane.transform.TransformDirection(new Vector3(randomPos2D.x, 0f, randomPos2D.y));
                    spawnPos += plane.transform.up * 0.02f;
                    Quaternion spawnRot = Quaternion.identity;

                 GameObject dust = Instantiate(randomDust, spawnPos, spawnRot);

                    dust.transform.Rotate(Vector3.forward * Random.Range(0f, 360f));
                    float randomScale = Random.Range(5f, 6f);
                    dust.transform.localScale = Vector3.one * randomScale;

                    DustClickHandler handler = dust.GetComponent<DustClickHandler>();
                    if (handler == null) handler = dust.AddComponent<DustClickHandler>();
                    handler.scoreValue = dustScore;

                    if (dust.GetComponent<Collider>() == null)
                        dust.AddComponent<SphereCollider>();

                    spawnedDusts.Add(dust);
                }

                // 일정 시간 후 삭제
                yield return new WaitForSeconds(dustLifetime);

                foreach (var dust in spawnedDusts)
                {
                    if (dust != null) Destroy(dust);
                }

                // 재생성 대기
                yield return new WaitForSeconds(respawnDelay);
            }
            else
            {
                yield return new WaitForSeconds(1f); // 랜덤 스폰 실패 시 대기
            }
        }
    }
    void Start()
    {
        if (planeManager == null)
            planeManager = GetComponent<ARPlaneManager>();

    }

    void OnEnable()
    {
        if (planeManager != null)
            planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        if (planeManager != null)
            planeManager.planesChanged -= OnPlanesChanged;
    }
}
