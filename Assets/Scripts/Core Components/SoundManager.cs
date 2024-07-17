using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
   // setting the sound manager as static so we dont need to get reference to it in other scripts
    public static SoundManager instance { get; private set; }
    private AudioSource source;
    private AudioSource musicSource; // Reference to the level music AudioSource
    private AudioSource effectSource; // reference to the game effects sound AudioSource
    private AudioSource menuSource; // reference to the menu effects sound AudioSource
    private AudioSource mainmusicSource; // Reference to the main level music AudioSource
    private AudioSource minigamemusicSource; // Reference to the main level music AudioSource

    // referencing scene function
    Scene scene;

    [Header("Level 1 Music")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private AudioClip pauseMusic;
    [SerializeField] private AudioClip deathMusic;
    [SerializeField] private AudioClip finishMusic;

    [Header("Level 2 Music")]
    [SerializeField] private AudioClip level2Music;

    [Header("Minigame Music")]
    [SerializeField] private AudioClip rhythmMusic1;

    private float skipTime = 21.62f;


    // on awake we get reference to the audio source and set the sound manager instance to this
    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        effectSource = transform.GetChild(1).GetComponent<AudioSource>();
        menuSource = transform.GetChild(2).GetComponent<AudioSource>();
        mainmusicSource = transform.GetChild(3).GetComponent<AudioSource>();
        minigamemusicSource = transform.GetChild(4).GetComponent<AudioSource>();

        // loading player preferences by changing volumes by 0
        ChangeMusicVolume(0);
        ChangeSFXVolume(0);
        ChangeMenuVolume(0);
    }

    private void Update()
    {
        scene = SceneManager.GetActiveScene();

        if (scene.buildIndex == 0 && !mainmusicSource.isPlaying)
        {
            PlayMainMenuMusic();
        }

        if (scene.buildIndex == 1 && !mainmusicSource.isPlaying && GameStateManager.Paused != true && GameStateManager.playerLost != true)
        {
            PlayMainMusic();
        }

        if (scene.buildIndex == 2 && !minigamemusicSource.isPlaying && GameStateManager.Paused != true && GameStateManager.playerLost != true && GameStateManager.minigameStart == true)
        {
            PlayRhythmMusic1();
        }

        if (scene.buildIndex == 3 && !mainmusicSource.isPlaying && GameStateManager.Paused != true && GameStateManager.playerLost != true)
        {
            PlayLevel2Music();
        }
    }

    #region Volume Change Functions
    private void ChangeSourceVolume(string volumeName, float change, AudioSource source)
    {
        // changing the initial volume value to the player preference value if loadable
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume += change;

        // making volume reset to 0 or 1 based on if we hit max or min value
        if (currentVolume > 1)
            currentVolume -= 1;
        else if (currentVolume < 0)
            currentVolume += 1;

        // Round the volume to the nearest 0.1 to avoid floating-point precision issues
        currentVolume = Mathf.Round(currentVolume * 10f) / 10f;

        // assigning the value
        source.volume = currentVolume;

        // saving the final value to player preference
        PlayerPrefs.SetFloat(volumeName, currentVolume);
    }

    public void ChangeMusicVolume(float _change)
    {
        ChangeSourceVolume("musicVolume", _change, musicSource);
        ChangeSourceVolume("mainmusicVolume", _change, mainmusicSource);
        ChangeSourceVolume("minigamemusicVolume", _change, minigamemusicSource);
    }

    public void ChangeSFXVolume(float _change)
    {
        ChangeSourceVolume("sfxVolume", _change, effectSource);
    }

    public void ChangeMenuVolume(float _change)
    {
        ChangeSourceVolume("menuVolume", _change, menuSource);
    }
    #endregion

    #region Game SFX Playback Functions
    // play an audio clip once
    public void PlaySound(AudioClip _sound)
    {
        effectSource.PlayOneShot(_sound);
    }

    // stop playing an audio clip that was set
    public void StopSound()
    {
        effectSource.Stop();
    }

    // pausing an audio clip that was set
    public void PauseSound()
    {
        effectSource.Pause();
    }

    // pausing an audio clip that was set
    public void UnPauseSound()
    {
        effectSource.UnPause();
    }
    #endregion

    #region Menu SFX Playback Functions
    // play a menu audio clip once
    public void PlayMenuSound(AudioClip _sound)
    {
        menuSource.PlayOneShot(_sound);
    }

    // stop playing a menu audio clip that was set
    public void StopMenuSound()
    {
        menuSource.Stop();
    }
    #endregion

    #region Music Playback (Pause/Death Screen Music) Functions
    // Play the pause music
    public void PlayPauseMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
            musicSource.PlayOneShot(pauseMusic);
    }

    // Play the death music
    public void PlayDeathMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
            musicSource.PlayOneShot(deathMusic);
    }

    // Stop the level music
    public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
            musicSource.Stop();
    }
    #endregion

    #region Main Music (Level Music) Playback Functions
    // pause playing a menu audio clip that was set
    public void PauseMainMusic()
    {
        mainmusicSource.Pause();
        minigamemusicSource.Pause();
    }

    // unpause playing a menu audio clip that was set
    public void UnPauseMainMusic()
    {
        mainmusicSource.UnPause();
        minigamemusicSource.UnPause();
    }

    // Stop the main level music
    public void StopMainMusic()
    {
        if (mainmusicSource != null && mainmusicSource.isPlaying)
        {
            mainmusicSource.Stop();
            minigamemusicSource.Stop();
        }
    }

    // Play the main level music
    public void PlayMainMusic()
    {
        if (mainmusicSource != null && !mainmusicSource.isPlaying)
            mainmusicSource.PlayOneShot(mainMusic);
    }

    public void PlayLevel2Music()
    {
        if(mainmusicSource != null && !mainmusicSource.isPlaying)
            mainmusicSource.PlayOneShot(level2Music);
    }

    public void PlayFinishMusic()
    {
        if (mainmusicSource != null && !mainmusicSource.isPlaying)
            mainmusicSource.PlayOneShot(finishMusic);
    }
    #endregion

    #region Main Menu Music
    public void PlayMainMenuMusic()
    {
        if (mainmusicSource != null && scene.buildIndex == 0)
        {
            mainmusicSource.clip = mainMenuMusic;
            mainmusicSource.Play();
        }
    }

    public void SkipMainMenuMusic()
    {
        if (mainmusicSource != null)
        {
            mainmusicSource.Stop();
            mainmusicSource.clip = mainMenuMusic;
            mainmusicSource.Play();
            mainmusicSource.time = skipTime;
        }
    }
    #endregion

    #region Minigame Music Playback Functions
    public void PlayRhythmMusic1()
    {
        if (minigamemusicSource != null && !minigamemusicSource.isPlaying)
            minigamemusicSource.PlayOneShot(rhythmMusic1);
    }
    #endregion
}
