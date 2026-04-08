using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (!sfxSource){
            sfxSource = GetComponent<AudioSource>();
            if(!sfxSource) sfxSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (!clip || !sfxSource) return;
        sfxSource.PlayOneShot(clip);
    }

    /// <summary>Assigns and plays a clip exclusively; optionally loops.</summary>
    public void PlayExclusiveSFX(AudioClip clip, bool loop = false)
    {
        if (!clip || !sfxSource) return;

        sfxSource.clip = clip;
        sfxSource.loop = loop;
        sfxSource.Play();
    }
    
    public void StopSFX()
    {
        if (!sfxSource) return;

        sfxSource.Stop();
        sfxSource.loop = false;
    }
}
