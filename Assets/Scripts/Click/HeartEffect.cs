using UnityEngine;

public class HeartEffect : MonoBehaviour
{
    public float fadeDuration = 0.5f;
    public float riseHeight = 0.5f;

    private SpriteRenderer spriteRenderer;
    private float timer;

    private Vector3 startPosition;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float progress = timer / fadeDuration;

        // 하트 올라가는 연출
        transform.position = startPosition + Vector3.up * riseHeight * progress;

        // 알파값 조절해서 페이드아웃
        Color color = spriteRenderer.color;
        color.a = Mathf.Lerp(1f, 0f, progress);
        spriteRenderer.color = color;

        // 다 사라지면 삭제
        if (progress >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
