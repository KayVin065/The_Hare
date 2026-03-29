using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource sfxSource;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void PlaySFX(AudioClip clip)
    {
        if(clip != null)
        sfxSource.PlayOneShot(clip);
    }
}
