using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // 소환할 물체 프리팹
    public Vector2 spawnRangeX = new Vector2(-5f, 5f); // X축 범위
    public Vector2 spawnRangeY = new Vector2(-5f, 5f); // Y축 범위
    public float objectLifetime = 5f; // 물체가 사라지는 시간
    public float spawnInterval = 5f; // 물체 생성 간격 (초)

    void Start()
    {
        // 일정 간격으로 물체 생성 시작
        InvokeRepeating("SpawnRandomObject", 0f, spawnInterval);
    }

    public void SpawnRandomObject()
    {
        // 랜덤 위치 설정 (카메라 뷰 기준, 필요시 3D는 Z축 추가 가능)
        float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
        float randomY = Random.Range(spawnRangeY.x, spawnRangeY.y);
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

        // 물체 생성
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

        // 일정 시간 후 소멸
        Destroy(spawnedObject, objectLifetime);
    }
}
