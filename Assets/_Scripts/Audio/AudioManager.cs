using UnityEngine;

// background music and gather sounds
public class AudioManager : MonoBehaviour
{
    [Header("Audio sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField, Range(0f, 1f)] private float musicVolume = 0.45f;

    [Header("Gather sounds")]
    [SerializeField] private AudioClip woodClip;
    [SerializeField] private AudioClip stoneClip;
    [SerializeField] private AudioClip berriesClip;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.92f, 1.08f);

    [Header("References")]
    [SerializeField] private PlayerGatherer gatherer;

    void OnEnable()
    {
        if (gatherer != null)
        {
            gatherer.Gathered += OnGathered;
        }
    }

    void OnDisable()
    {
        if (gatherer != null)
        {
            gatherer.Gathered -= OnGathered;
        }
    }

    void Start()
    {
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
    }

    private void OnGathered(ResourceType type)
    {
        AudioClip clip = ClipFor(type);
        if (clip == null || sfxSource == null)
        {
            return;
        }

        // slightly different pitch each time
        sfxSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        sfxSource.PlayOneShot(clip);
    }

    private AudioClip ClipFor(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Wood:
                return woodClip;
            case ResourceType.Stone:
                return stoneClip;
            case ResourceType.Berries:
                return berriesClip;
            default:
                return null;
        }
    }
}
