using UnityEngine;
using UnityEngine.Events;

public class Pause_Resume : MonoBehaviour
{
    private bool isPaused;
    public UnityEvent PauseEvents;
    public UnityEvent ResumeEvents;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        PauseEvents.Invoke();
        Debug.Log("Paused");
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        ResumeEvents.Invoke();
        Debug.Log("Resumed");
    }
}
