using UnityEngine;
using UnityEngine.Events;

public class EventCollider : MonoBehaviour
{
    public UnityEvent CollideEvents;
    public void DisableCollider()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void EnableCollider()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            CollideEvents?.Invoke();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollideEvents?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DisableCollider();
        }
    }
}
