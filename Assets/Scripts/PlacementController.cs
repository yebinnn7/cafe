using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class PlacementController : MonoBehaviour
{
    public static PlacementController instance;

    public GameObject objectToSpawn; // 생성할 오브젝트
    public Button spawnButton; // 버튼

    private GameObject spawnedObject; // 소환된 오브젝트
    public bool isFollowingMouse = false; // 오브젝트가 마우스를 따라다니는지 여부

    // ======================= test ===========================
    private bool isShaking = false; // 흔들림 상태
    private Vector3 originalPosition; // 오브젝트 원래 위치
    public float durability = 100f; // 초기 내구도

    private void Awake()
    {
        instance = this; // 싱글톤 패턴 적용
    }

    void Start()
    {

        // 버튼 클릭 이벤트 등록
        spawnButton.onClick.AddListener(SpawnObject);
    }

    void Update()
    {
        // 소환된 오브젝트가 마우스를 따라다니도록 위치 업데이트
        if (spawnedObject != null && isFollowingMouse)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // 2D에서는 z축은 0으로 고정
            spawnedObject.transform.position = mousePosition;

            // 마우스 클릭 시 오브젝트 고정
            if (Input.GetMouseButtonDown(0)) // 왼쪽 마우스 버튼
            {
                isFollowingMouse = false; // 마우스 따라다니기 비활성화
                Destroy(spawnedObject);
                spawnedObject = Instantiate(objectToSpawn);
                spawnedObject.transform.position = mousePosition;
                // 오브젝트의 초기 위치 저장
                originalPosition = mousePosition;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) // 왼쪽 마우스 버튼
            {
                // 클릭 시 내구도 감소
                ReduceDurability(10f);
                Debug.Log("Object Broken!");
                // 흔들림 효과 시작
                if (!isShaking)
                    StartCoroutine(Shake());
            }
        }
    }

    void SpawnObject()
    {
        Destroy(spawnedObject);
        // 새로운 오브젝트 소환 및 마우스 따라다니도록 설정
        spawnedObject = Instantiate(objectToSpawn);
        isFollowingMouse = true; // 마우스 따라다니기 활성화
    }

    void ReduceDurability(float amount)
    {
        durability -= amount;
        if (durability < 0)
            durability = 0;

        // 내구도가 0이면 파괴 처리
        if (durability == 0)
        {
            Destroy(spawnedObject, 0.5f); // 0.5초 뒤 오브젝트 파괴
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
            spawnedObject.transform.position = originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spawnedObject.transform.position = originalPosition;
        isShaking = false;
    }
}
