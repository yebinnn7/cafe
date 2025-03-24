using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn; // 소환할 프리팹들
    public GameObject heartPrefab; // 하트 이펙트 프리팹
    public Vector2 spawnRangeX = new Vector2(-1.25f, 1.25f);
    public Vector2 spawnRangeY = new Vector2(-1f, 1.4f);
    public float objectLifetime = 5f;
    public float spawnInterval = 5f;

    private int likeabilityScore = 100;

    void Start()
    {
        InvokeRepeating("SpawnRandomObject", 0f, spawnInterval);
    }

    public void SpawnRandomObject()
    {
        // 현재 위치 기준으로 랜덤 위치 설정
        float randomX = transform.position.x + Random.Range(spawnRangeX.x, spawnRangeX.y);
        float randomY = Random.Range(spawnRangeY.x, spawnRangeY.y);
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

        // 랜덤 프리팹 선택
        GameObject randomPrefab = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

        // 물체 생성 및 파괴 콜백 설정
        GameObject spawnedObject = Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
        ClickableObject clickable = spawnedObject.AddComponent<ClickableObject>();
        clickable.SetLifetime(objectLifetime);
        clickable.OnDestroyed += HandleObjectDestroyed;
    }

    private void HandleObjectDestroyed(bool wasClicked, Vector3 objectPosition)
    {
        if (!wasClicked)
        {
            likeabilityScore -= 10;
            Debug.Log("물체가 클릭되지 않았습니다! 현재 호감도: " + likeabilityScore);
        }

        Instantiate(heartPrefab, objectPosition, Quaternion.identity);
    }
}