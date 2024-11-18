using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject prefabToPlace;
    private GameObject previewObject;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.point;

            if (previewObject == null)
            {
                previewObject = Instantiate(prefabToPlace);
                // 투명하게 만들어서 미리보기로 사용
                // Material을 변경하는 로직 추가 가능
            }

            previewObject.transform.position = targetPosition;

            if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭
            {
                Instantiate(prefabToPlace, targetPosition, Quaternion.identity);
            }
        }
    }
}
