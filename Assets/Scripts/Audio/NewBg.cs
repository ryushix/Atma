using UnityEngine;

public class NewSceneManager : MonoBehaviour
{
    public AudioClip newBackgroundMusic;

    void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ChangeMusic(newBackgroundMusic);
        }
    }
}
