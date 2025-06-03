using UnityEngine;

public class TriggerControlls : MonoBehaviour
{
    public GameObject UIObject;
    public GameObject triggerArea;
    void Start()
    {
        UIObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UIObject.SetActive(true);
            Debug.Log("Work");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UIObject.SetActive(false);
            Destroy(triggerArea); 
        }
        
    }
}
