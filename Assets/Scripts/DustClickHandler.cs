using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Renderer))]
public class DustClickHandler : MonoBehaviour, IPointerClickHandler
{
    private int clickCount = 0;
    private Renderer rend;
    private Color originalColor;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickCount++;

        // 알파값 감소
        float alpha = Mathf.Clamp01(1f - (clickCount / 3f));
        Color newColor = originalColor;
        newColor.a = alpha;
        rend.material.color = newColor;

        if (clickCount >= 3)
        {
            Destroy(gameObject);
        }
    }
}