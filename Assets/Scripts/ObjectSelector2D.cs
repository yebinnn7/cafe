using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectSelector2D : MonoBehaviour
{
    public Slider durabilityBar; // 내구도 바
    public float durability = 100f; // 초기 내구도
    private bool isShaking = false; // 흔들림 상태
    private Vector3 originalPosition; // 오브젝트 원래 위치

    void Start()
    {
        // 오브젝트의 초기 위치 저장
        originalPosition = transform.position;
        // 내구도 바 초기화
        if (durabilityBar != null)
            durabilityBar.value = durability / 100f;
    }

    void OnMouseDown()
    {
        // 클릭 시 내구도 감소
        ReduceDurability(10f);
        // 흔들림 효과 시작
        if (!isShaking)
            StartCoroutine(Shake());
    }

    void ReduceDurability(float amount)
    {
        durability -= amount;
        if (durability < 0)
            durability = 0;

        // 내구도 바 업데이트
        if (durabilityBar != null)
            durabilityBar.value = durability / 100f;

        // 내구도가 0이면 파괴 처리
        if (durability == 0)
        {
            Debug.Log("Object Broken!");
            Destroy(gameObject, 0.5f); // 0.5초 뒤 오브젝트 파괴
        }
    }

    System.Collections.IEnumerator Shake()
    {
        isShaking = true;

        float elapsedTime = 0f;
        float duration = 0.3f; // 흔들림 지속 시간
        float magnitude = 0.1f; // 흔들림 강도

        while (elapsedTime < duration)
        {
            float offsetX = Random.Range(-magnitude, magnitude);
            float offsetY = Random.Range(-magnitude, magnitude);
            transform.position = originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        isShaking = false;
    }
}
