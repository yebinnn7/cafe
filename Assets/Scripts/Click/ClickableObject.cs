using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public delegate void ObjectDestroyed(bool wasClicked, Vector3 position);
    public event ObjectDestroyed OnDestroyed;

    private float lifetime;
    private bool wasClicked = false;

    public void SetLifetime(float time)
    {
        lifetime = time;
        Invoke("DestroyObject", lifetime);
    }

    private void OnMouseDown()
    {
        wasClicked = true;
        DestroyObject();
    }

    private void DestroyObject()
    {
        OnDestroyed?.Invoke(wasClicked, transform.position);
        Destroy(gameObject);
    }
}
