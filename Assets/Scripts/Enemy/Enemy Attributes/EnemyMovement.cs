using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D enemyRb;
    private Collider2D playerCollider;

    public Transform leftPoint;
    public Transform rightPoint;
    public float moveSpeed = 5f;
    private bool movingRight = true;

    public Transform playerTransform;
    public float chaseRadius = 7f;

    [Header("Physics Material Settings")]
    public PhysicsMaterial2D highFrictionMaterial;
    public PhysicsMaterial2D lowFrictionMaterial;

    void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
    }

    public void Patrol()
    {
        if (movingRight)
        {
            enemyRb.linearVelocity = new Vector2(moveSpeed, enemyRb.linearVelocity.y);
            if (transform.position.x >= rightPoint.position.x)
            {
                Flip();
            }
        }
        else
        {
            enemyRb.linearVelocity = new Vector2(-moveSpeed, enemyRb.linearVelocity.y);
            if (transform.position.x <= leftPoint.position.x)
            {
                Flip();
            }
        }
    }

    public void Chase()
    {
        if (playerTransform == null) return;

        float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);
        enemyRb.linearVelocity = new Vector2(direction * moveSpeed, enemyRb.linearVelocity.y);
        if ((direction > 0 && !movingRight) || (direction < 0 && movingRight))
        {
            Flip();
        }
    }

    public bool IsPlayerInRange()
    {
        if (playerTransform == null) return false;
        return Vector2.Distance(transform.position, playerTransform.position) <= chaseRadius;
    }

    private void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void StopMovement()
    {
        enemyRb.linearVelocity = Vector2.zero;
    }

    public void SetHighFriction()
    {
        if (playerCollider != null)
        {
            playerCollider.sharedMaterial = highFrictionMaterial;
        }
    }

    public void SetLowFriction()
    {
        if (playerCollider != null)
        {
            playerCollider.sharedMaterial = lowFrictionMaterial;
        }
    }
}
