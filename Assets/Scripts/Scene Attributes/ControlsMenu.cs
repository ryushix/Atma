using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsMenu : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }
}
