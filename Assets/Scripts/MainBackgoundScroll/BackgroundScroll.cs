using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float speed = 0.5f; // 스크롤 속도
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // 시간 흐름에 따라 offset 값을 증가시킴
        float offset = Time.time * speed;

        // 텍스처의 x좌표(offset)를 이동
        rend.material.mainTextureOffset = new Vector2(offset, 0);
    }
}