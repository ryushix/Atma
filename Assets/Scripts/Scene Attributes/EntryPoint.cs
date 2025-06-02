using UnityEngine;
using UnityEngine.SceneManagement;


public class EntryPoint : MonoBehaviour
{
    public string targetSceneName;
    private bool isPlayerNearby = false;

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (!string.IsNullOrEmpty(targetSceneName))
            {
                SceneManager.LoadSceneAsync(targetSceneName);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log(isPlayerNearby);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
