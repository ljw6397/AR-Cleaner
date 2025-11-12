using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DustSpawner : MonoBehaviour
{
    [Header("AR Components")]
    public ARPlaneManager planeManager;

    [Header("Dust Prefabs (랜덤 선택)")]
    public List<GameObject> dustPrefabs = new List<GameObject>();

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

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
        {
            if (dustPrefabs.Count == 0) return;

            if (Random.value < spawnChance)
            {
                int dustCount = Random.Range(minDustCount, maxDustCount + 1);

                for (int i = 0; i < dustCount; i++)
                {
                    GameObject randomDust = dustPrefabs[Random.Range(0, dustPrefabs.Count)];

                    Vector2 randomPos2D = Random.insideUnitCircle * spawnRadius;
                    Vector3 spawnPos = plane.center + plane.transform.TransformDirection(new Vector3(randomPos2D.x, 0f, randomPos2D.y));

                    spawnPos += plane.transform.up * 0.01f;

                    Quaternion spawnRot = plane.transform.rotation;
                    GameObject dust = Instantiate(randomDust, spawnPos, spawnRot, plane.transform);

                    dust.transform.LookAt(Camera.main.transform, plane.transform.up);

                    dust.transform.position += dust.transform.forward * 0.005f;

                    dust.transform.Rotate(Vector3.forward * Random.Range(0f, 360f));
                    float randomScale = Random.Range(0.05f, 0.15f);
                    dust.transform.localScale = Vector3.one * randomScale;

                    if (dust.GetComponent<DustClickHandler>() == null)
                    {
                        dust.AddComponent<DustClickHandler>();
                    }

                    if (dust.GetComponent<Collider>() == null)
                    {
                        dust.AddComponent<SphereCollider>();
                    }
                }
            }
        }
    }
}
