using Unity.VisualScripting;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // 소환할 프리팹
    public Vector2 spawnRangeX = new Vector2(-5f, 5f); // X축 범위
    public Vector2 spawnRangeY = new Vector2(-5f, 5f); // Y축 범위
    public float objectLifetime = 5f; // 물체가 사라지는 시간
    public float spawnInterval = 5f; // 물체 생성 간격 (초)

    private int likeabilityScore = 100; // 초기 호감도 점수

    void Start()
    {
        // 일정 간격으로 물체 생성 시작
        InvokeRepeating("SpawnRandomObject", 0f, spawnInterval);
    }

    public void SpawnRandomObject()
    {
        // 랜덤 위치 설정
        float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
        float randomY = Random.Range(spawnRangeY.x, spawnRangeY.y);
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

        // 물체 생성 및 파괴 콜백 설정
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        ClickableObject clickable = spawnedObject.AddComponent<ClickableObject>();
        clickable.SetLifetime(objectLifetime);
        clickable.OnDestroyed += HandleObjectDestroyed;
    }

    private void HandleObjectDestroyed(bool wasClicked)
    {
        if (!wasClicked)
        {
            // 물체가 클릭되지 않았을 경우 호감도 감소
            likeabilityScore -= 10;
            Debug.Log("물체가 클릭되지 않았습니다! 현재 호감도: " + likeabilityScore);
        }
    }
}
