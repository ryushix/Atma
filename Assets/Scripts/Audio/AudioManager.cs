using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;

    [Header("Audio Clip")]
    public AudioClip background;
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void ChangeMusic(AudioClip newClip)
{
    if (musicSource.clip == newClip) return; // Tidak perlu ganti jika sama

    musicSource.Stop();
    musicSource.clip = newClip;
    musicSource.Play();
}
}
