using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Prolog");
    }

    public void OpenControls()
    {
        SceneManager.LoadSceneAsync("Controls");
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif

        Debug.Log("Keluar dari game.");
    }
}
