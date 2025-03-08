using UnityEngine;

public class RandomAxisMoveCharacter : MonoBehaviour
{
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite sideSprite;

    public float moveDistance = 1f;
    public float moveInterval = 1f;

    private SpriteRenderer spriteRenderer;
    private Vector2 targetPosition;
    private bool isStopped = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPosition = transform.position;
        InvokeRepeating(nameof(SetRandomTargetPosition), 0f, moveInterval);
    }

    private void Update()
    {
        if (!isStopped)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveDistance * Time.deltaTime);
        }
    }

    private void SetRandomTargetPosition()
    {
        if (isStopped) return;

        Vector2 currentPosition = transform.position;
        Vector2 newPosition = currentPosition;
        Vector2 moveDirection;

        if (Random.value > 0.5f)
        {
            float directionY = Random.value > 0.5f ? 1f : -1f;
            newPosition.y += directionY * moveDistance;
            moveDirection = Vector2.up * directionY;
        }
        else
        {
            float directionX = Random.value > 0.5f ? 1f : -1f;
            newPosition.x += directionX * moveDistance;
            moveDirection = Vector2.right * directionX;
        }

        targetPosition = newPosition;

        if (moveDirection.y > 0)
        {
            spriteRenderer.sprite = backSprite;
            spriteRenderer.flipX = false;
        }
        else if (moveDirection.y < 0)
        {
            spriteRenderer.sprite = frontSprite;
            spriteRenderer.flipX = false;
        }
        else if (moveDirection.x != 0)
        {
            spriteRenderer.sprite = sideSprite;
            spriteRenderer.flipX = moveDirection.x > 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isStopped = true;
        CancelInvoke(nameof(SetRandomTargetPosition));
        Invoke(nameof(ResumeMovement), 1f);
    }

    private void ResumeMovement()
    {
        isStopped = false;
        InvokeRepeating(nameof(SetRandomTargetPosition), 0f, moveInterval);
    }
}
