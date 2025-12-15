using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScroll : MonoBehaviour
{
    public float speed = 3f;      // 이동 속도
    public float width = 10f;     // 배경 이미지의 가로 길이 (유니티 좌표 기준)

    void Update()
    {
        // 왼쪽으로 계속 이동
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // 만약 위치가 왼쪽 끝(너비만큼)을 벗어나면
        if (transform.position.x <= -width)
        {
            // 오른쪽 끝으로 위치 재조정 (2 * width 만큼 이동)
            transform.position += new Vector3(width * 2, 0, 0);
        }
    }
}
