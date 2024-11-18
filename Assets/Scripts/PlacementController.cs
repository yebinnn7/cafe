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
}
