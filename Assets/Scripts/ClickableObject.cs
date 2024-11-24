using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public delegate void ObjectDestroyedHandler(bool wasClicked);
    public event ObjectDestroyedHandler OnDestroyed;

    private bool wasClicked = false;

    public void SetLifetime(float lifetime)
    {
        // 일정 시간이 지난 후 자동으로 파괴
        Invoke("DestroySelf", lifetime);
    }

    public void OnMouseDown()
    {
        // 물체가 클릭되었을 때 처리
        wasClicked = true;
        DestroySelf();
    }

    private void DestroySelf()
    {
        // 파괴될 때 클릭 여부를 이벤트로 전달
        OnDestroyed?.Invoke(wasClicked);
        Destroy(gameObject);
    }
}
