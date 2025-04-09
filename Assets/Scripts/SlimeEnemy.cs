using UnityEngine;
using System.Collections;

public class SlimeEnemy : MonoBehaviour
{
    public float jumpForce = 7f;
    public float moveSpeed = 2f;
    public float jumpCooldown = 1.0f;
    public float chaseSpeed = 3.5f;
    public float detectionRadius = 5f;
    
    public Transform rightPoint;
    public Transform leftPoint;

    private Rigidbody2D rb;
    private bool movingRight = true;
    private bool isJumping = false;
    private Transform player;
    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(JumpCooldown());
    }

    void Update()
    {
        float playerDistance = Vector2.Distance(transform.position, player.position);

        if (playerDistance <= detectionRadius)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (!isJumping && IsGrounded())
        {
            StartCoroutine(JumpCooldown());
        }
    }

    IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(jumpCooldown);

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Jump();
        }

        isJumping = false;
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2((movingRight ? 1 : -1) * moveSpeed, jumpForce);
    }

    void ChasePlayer()
    {
        float direction = (player.position.x > transform.position.x) ? 1 : -1;

        if ((direction == 1 && !movingRight) || (direction == -1 && movingRight))
        {
            Flip();
        }

        rb.linearVelocity = new Vector2(direction * chaseSpeed, jumpForce);
    }


    private bool IsGrounded()
    {
        return rb.linearVelocity.y == 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TurnPoint"))
        {
            Flip();
        }
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
}
