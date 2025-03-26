using UnityEngine;

public class EnemyWalker : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform rightPoint;
    public Transform leftPoint;
    public float chaseSpeed = 3.5f;
    public float detectionRadius = 5f;
    
    private Rigidbody2D rb;
    private bool movingRight = true;
    private Transform player;
    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
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

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        float moveDirection = movingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

    void ChasePlayer()
    {
        float direction = (player.position.x > transform.position.x) ? 1 : -1;

        if ((direction == 1 && !movingRight) || (direction == -1 && movingRight))
        {
            Flip();
        }

        rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);
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
