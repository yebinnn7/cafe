using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    void OnMouseDown()
    {
        // 오브젝트를 클릭 시 즉시 제거
        Destroy(gameObject);
    }
}
