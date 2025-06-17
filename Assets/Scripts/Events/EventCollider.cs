using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;

public class EventCollider : MonoBehaviour
{
    public UnityEvent CollideEvents;
    public UnityEvent UnCollideEvents;
    public UnityEvent InteractionEvents;
    private bool isPlayerCollide;

    private void Update()
    {
        if (isPlayerCollide && Input.GetKeyDown(KeyCode.E))
        {
            InteractionEvents?.Invoke();
        }
    }
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
            isPlayerCollide = true;
            CollideEvents?.Invoke();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerCollide = true;
            CollideEvents?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerCollide = false;
            UnCollideEvents?.Invoke();
        }
    }
}
