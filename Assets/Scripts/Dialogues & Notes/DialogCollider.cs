using UnityEngine;
using UnityEngine.Events;

public class DialogCollider : MonoBehaviour
{
    public UnityEvent CollideEvents;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            CollideEvents.Invoke();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollideEvents.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            this.enabled = false; 
        }
    }
}
