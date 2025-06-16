using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D enemyRb;
    private Collider2D playerCollider;
    public Transform playerTransform;

    [Header("Direction")]
    public Transform leftPoint;
    public Transform rightPoint;
    public float chaseRadius = 7f;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public bool canMove = true;
    public float idleTime = 2f;
    private bool movingRight = true;

    [Header("Bounce Settings")]
    public float bounceForce = 7f;
    public bool enableBounce = true;
    private bool isGrounded = true;


    [Header("Physics Material Settings")]
    public PhysicsMaterial2D highFrictionMaterial;
    public PhysicsMaterial2D lowFrictionMaterial;

    void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
    }

    public void Patrol()
    {
        if (!canMove) return;

        float direction = movingRight ? 1f : -1f;
        enemyRb.linearVelocity = new Vector2(direction * moveSpeed, enemyRb.linearVelocity.y);

        // Debug.Log(enableBounce && isGrounded);
        if (enableBounce && isGrounded)
        {
            Bounce();
            StartCoroutine(TemporaryIdle());
        }
    }

    public void Chase()
    {
        if (playerTransform == null || !canMove) return;

        float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);
        enemyRb.linearVelocity = new Vector2(direction * moveSpeed, enemyRb.linearVelocity.y);


        if ((direction > 0 && !movingRight) || (direction < 0 && movingRight))
        {
            Flip();
        }

        if (enableBounce && isGrounded)
        {
            Bounce();
            StartCoroutine(TemporaryIdle());
        }
    }

    private void Bounce()
    {
        enemyRb.linearVelocity = new Vector2(enemyRb.linearVelocity.x, 0f); // reset Y to prevent buildup
        enemyRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        isGrounded = false;
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

    private IEnumerator TemporaryIdle()
    {
        canMove = false;
        yield return new WaitForSeconds(idleTime);
        canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
                if (movingRight && transform.position.x >= rightPoint.position.x)
            {
                Flip();
            }
            else if (!movingRight && transform.position.x <= leftPoint.position.x)
            {
                Flip();
            }
            isGrounded = true;
        }
    }
}
