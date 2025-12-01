using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Renderer))]
public class DustClickHandler : MonoBehaviour, IPointerClickHandler
{
    private int clickCount = 0;
    private Renderer rend;
    private Color originalColor;

    public int scoreValue = 1; // 생성 시 DustSpawner에서 변경 가능

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickCount++;

        float alpha = Mathf.Clamp01(1f - (clickCount / 3f));
        Color newColor = originalColor;
        newColor.a = alpha;
        rend.material.color = newColor;

        if (clickCount >= 3)
        {
            ScoreManager.Instance.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }
}