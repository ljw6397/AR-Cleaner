using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Renderer))]
public class DustClickHandler : MonoBehaviour, IPointerClickHandler
{
    private int clickCount = 0;
    private Renderer rend;
    private Color originalColor;

    public int scoreValue = 1;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.GetColor("_BaseColor");  // ← URP Lit 컬러 읽기
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickCount++;

        if (clickCount == 1)
        {
            SetURPTransparent(rend.material);
            DustEffectManager.Instance.PlayBigDust(transform.position);
            SoundManager.Instance.PlaySqueak();
        }
        else if (clickCount == 2)
        {
            DustEffectManager.Instance.PlaySmallDust(transform.position);
            SoundManager.Instance.PlaySqueak();
        }

        float alpha = Mathf.Clamp01(1f - (clickCount / 3f));
        Color newColor = originalColor;
        newColor.a = alpha;
        rend.material.SetColor("_BaseColor", newColor);

        if (clickCount >= 3)
        {
            DustEffectManager.Instance.PlayCleanFinish(transform.position);
            SoundManager.Instance.PlayCleanFinish();

            ScoreManager.Instance.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }

    void SetURPTransparent(Material mat)
    {
        // Surface Type = Transparent
        mat.SetFloat("_Surface", 1.0f);

        // Blending 설정
        mat.SetFloat("_Blend", 0);   // Alpha blending
        mat.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

        // ZWrite 끄기
        mat.SetFloat("_ZWrite", 0);

        // 키워드 설정
        mat.DisableKeyword("_SURFACE_TYPE_OPAQUE");
        mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");

        mat.renderQueue = 3000;
    }
}