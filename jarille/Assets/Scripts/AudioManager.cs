using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;

    [Header("Music Lists")]
    public AudioClip[] mainMenuTracks;
    public AudioClip[] roomTracks;
    public AudioClip[] combatTracks;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayMainMenu()
    {
        PlayRandom(mainMenuTracks);
    }

    public void PlayRoom()
    {
        PlayRandom(roomTracks);
    }

    public void PlayCombat()
    {
        PlayRandom(combatTracks);
    }
}