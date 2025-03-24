using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn; // ��ȯ�� �����յ�
    public GameObject heartPrefab; // ��Ʈ ����Ʈ ������
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
        // ���� ��ġ �������� ���� ��ġ ����
        float randomX = transform.position.x + Random.Range(spawnRangeX.x, spawnRangeX.y);
        float randomY = Random.Range(spawnRangeY.x, spawnRangeY.y);
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

        // ���� ������ ����
        GameObject randomPrefab = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

        // ��ü ���� �� �ı� �ݹ� ����
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
            Debug.Log("��ü�� Ŭ������ �ʾҽ��ϴ�! ���� ȣ����: " + likeabilityScore);
        }

        Instantiate(heartPrefab, objectPosition, Quaternion.identity);
    }
}