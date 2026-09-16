using UnityEngine;
using UnityEngine.UI;

public class StartPanelManager : MonoBehaviour
{
    public static StartPanelManager Instance;

    [Header("UI")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Slider volumeSlider;

    [Header("Звук нажатия")]
    [Tooltip("Для мышки. Больше нигде не используется!")]
    [SerializeField] private AudioSource clickAudioSource;
    [SerializeField] private AudioClip clickSound;

    [Header("Фоновая музыка")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioClip backgroundMusic;

    private bool _isMusicStarted = false;
    private float _musicVolume = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        _musicVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = _musicVolume;
        startPanel.SetActive(true);
        Time.timeScale = 0f;

        playButton.onClick.AddListener(OnPlayClicked);
        exitButton.onClick.AddListener(Exit);

        musicAudioSource.loop = true;
        musicAudioSource.playOnAwake = false;
        musicAudioSource.volume = volumeSlider.value;

        volumeSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void OnPlayClicked()
    {
        PlayClickSound();

        if (!_isMusicStarted)
        {
            PlayBackgroundMusic();
            _isMusicStarted = true;
        }

        if (startPanel != null)
            startPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        PlayClickSound();
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void PlayClickSound()
    {
        if (clickAudioSource != null && clickSound != null)
        {
            clickAudioSource.PlayOneShot(clickSound);
        }
    }

    private void PlayBackgroundMusic()
    {
        if (musicAudioSource != null && backgroundMusic != null)
        {
            if (!musicAudioSource.isPlaying)
            {
                musicAudioSource.clip = backgroundMusic;
                musicAudioSource.Play();
            }
        }
    }

    public void StopMusic()
    {
        if (musicAudioSource != null && musicAudioSource.isPlaying)
        {
            musicAudioSource.Stop();
            _isMusicStarted = false;
        }
    }

    public void SetMusicVolume(float value)
    {
        _musicVolume = value;
        musicAudioSource.volume = _musicVolume;

        PlayerPrefs.SetFloat("Volume", _musicVolume);
        PlayerPrefs.Save();
    }
}