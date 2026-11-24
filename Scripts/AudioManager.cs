using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer & Snapshots")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerSnapshot normalSnapshot;
    [SerializeField] private AudioMixerSnapshot pausedSnapshot;

    [Header("Music Tracks")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private float crossfadeDuration = 1.0f;

    [Header("Sound Effects (8+ Events)")]
    public AudioClip jumpClip;
    public AudioClip[] hitClips; // 2-3 клипа для вариации удара
    public AudioClip pickupClip;
    public AudioClip enemyWindUpClip;
    public AudioClip deathClip;
    public AudioClip uiClickClip;
    public AudioClip winClip;
    public AudioClip loseClip;

    private AudioSource musicSourceA;
    private AudioSource musicSourceB;
    private AudioSource sfxSource;
    private bool isUsingSourceA = true;
    private AudioClip currentTrack;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Создаём 2 источника для кроссфейда музыки и 1 для SFX, все привязаны к группам микшера
        musicSourceA = gameObject.AddComponent<AudioSource>();
        musicSourceA.outputAudioMixerGroup = musicGroup;
        musicSourceA.loop = true;

        musicSourceB = gameObject.AddComponent<AudioSource>();
        musicSourceB.outputAudioMixerGroup = musicGroup;
        musicSourceB.loop = true;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.outputAudioMixerGroup = sfxGroup;
    }

    private void Start()
    {
        // Применяем сохранённую громкость в децибелах при запуске
        if (SaveManager.Instance != null)
        {
            ApplyMusicVolume(SaveManager.Instance.data.musicVolume);
            ApplySFXVolume(SaveManager.Instance.data.sfxVolume);
        }

        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetPausedSnapshot(false);

        AudioClip targetClip = (scene.name == "MainMenu") ? menuMusic : levelMusic;
        if (targetClip != null && targetClip != currentTrack)
        {
            StartCoroutine(CrossfadeMusic(targetClip));
        }
    }

    private IEnumerator CrossfadeMusic(AudioClip newClip)
    {
        currentTrack = newClip;
        AudioSource activeSource = isUsingSourceA ? musicSourceA : musicSourceB;
        AudioSource newSource = isUsingSourceA ? musicSourceB : musicSourceA;
        isUsingSourceA = !isUsingSourceA;

        newSource.clip = newClip;
        newSource.volume = 0f;
        newSource.Play();

        float timer = 0f;
        float startVol = activeSource.volume;

        while (timer < crossfadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / crossfadeDuration;
            newSource.volume = Mathf.Lerp(0f, 1f, t);
            activeSource.volume = Mathf.Lerp(startVol, 0f, t);
            yield return null;
        }

        newSource.volume = 1f;
        activeSource.Stop();
    }

    // Логарифмическая громкость в децибелах (Log10 * 20)
    public void ApplyMusicVolume(float sliderValue)
    {
        float clamped = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        float dB = Mathf.Log10(clamped) * 20f;
        if (mainMixer != null) mainMixer.SetFloat("MusicVol", dB);
    }

    public void ApplySFXVolume(float sliderValue)
    {
        float clamped = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        float dB = Mathf.Log10(clamped) * 20f;
        if (mainMixer != null) mainMixer.SetFloat("SFXVol", dB);
    }

    public void SetPausedSnapshot(bool isPaused)
    {
        if (isPaused && pausedSnapshot != null)
            pausedSnapshot.TransitionTo(0.25f);
        else if (!isPaused && normalSnapshot != null)
            normalSnapshot.TransitionTo(0.25f);
    }

    // Универсальный метод проигрывания SFX с вариацией pitch (0.92 - 1.08)
    public void PlaySFX(AudioClip clip, bool randomizePitch = true)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.pitch = randomizePitch ? Random.Range(0.92f, 1.08f) : 1f;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayJump() => PlaySFX(jumpClip, true);
    public void PlayPickup() => PlaySFX(pickupClip, true);
    public void PlayEnemyWindUp() => PlaySFX(enemyWindUpClip, true);
    public void PlayDeath() => PlaySFX(deathClip, false);
    public void PlayUIClick() => PlaySFX(uiClickClip, false);
    public void PlayWin() => PlaySFX(winClip, false);
    public void PlayLose() => PlaySFX(loseClip, false);

    public void PlayHit()
    {
        if (hitClips != null && hitClips.Length > 0)
        {
            AudioClip clip = hitClips[Random.Range(0, hitClips.Length)];
            PlaySFX(clip, true);
        }
    }
}