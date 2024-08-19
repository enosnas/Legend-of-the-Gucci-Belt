using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Playermanager { get; private set; }
    private bool canPause;
    // referencing scene function
    Scene scene;

    [Header("Game Over Screen")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause Screen")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject controlScreen;
    [SerializeField] private AudioClip pauseSound;
    [SerializeField] private AudioClip menuconfirmSound;
    [SerializeField] private AudioClip sfxtestSound;

    [Header("Speedrun Timer")]
    [SerializeField] private GameObject timer;

    [Header("Pause Screen Items")]
    [SerializeField] private Behaviour[] options;

    [Header("Controls Screen Items")]
    [SerializeField] private Behaviour[] controls;

    // [Header("Finish Screen")]
    // [SerializeField] private GameObject finishScreen;
    // [SerializeField] private AudioClip finishSound;

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        controlScreen.SetActive(false);
        timer.SetActive(false);
    }

    // activating and deactivating pause when hitting escape, changing audio as well
    // disabling pause menu in main menu also
    private void Update()
    {
        if (scene.buildIndex != 0 && scene.buildIndex != 2)
            canPause = true;
        else
            canPause = false;

        #region Pause in Menu Prevention/In Game Allowance
        // making the current scene set to the scene variable
        scene = SceneManager.GetActiveScene();

        if (canPause == true && controlScreen.activeInHierarchy == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && GameStateManager.playerLost != true)
            {
                // exit pause when hitting escape again
                if (pauseScreen.activeInHierarchy)
                {
                    SoundManager.instance.StopMusic();
                    SoundManager.instance.UnPauseSound();
                    SoundManager.instance.UnPauseMainMusic();
                    PauseGame(false);
                }
                // entering pause with escape
                else
                {
                    SoundManager.instance.PauseMainMusic();
                    SoundManager.instance.PauseSound();
                    SoundManager.instance.PlayPauseMusic();
                    PauseGame(true);
                }
            }
        }

        if(scene.buildIndex == 1)
            timer.SetActive(true);
        else
            timer.SetActive(false);
        #endregion
    }

    #region Game Over Screen and Buttons
    // activate game over screen
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        GameStateManager.playerLost = true;
        SoundManager.instance.StopMusic();
        SoundManager.instance.StopSound();
        SoundManager.instance.StopMainMusic();
        SoundManager.instance.PlayDeathMusic();
    }

    // game over functions
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameStateManager.playerLost = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        PauseGame(false);
        GameStateManager.playerLost = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion

    #region Pause Menu and Buttons
    public void PauseGame(bool _status)
    {
        // pausing and un-pausing 
        pauseScreen.SetActive(_status);

        // tracking pause status with the global variable in gamestatemanager
        GameStateManager.Paused = _status;

        if (_status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void ResumeGame()
    {
        SoundManager.instance.StopMusic();
        SoundManager.instance.UnPauseMainMusic();
        PauseGame(false);

    }

    public void SFXVolume()
    {
        SoundManager.instance.PlaySound(sfxtestSound);
        SoundManager.instance.ChangeSFXVolume(0.1f);
    }

    public void MusicVolume()
    {
        SoundManager.instance.PlayMenuSound(menuconfirmSound);
        SoundManager.instance.ChangeMusicVolume(0.1f);
    }

    public void MenuVolume()
    {
        SoundManager.instance.PlayMenuSound(menuconfirmSound);
        SoundManager.instance.ChangeMenuVolume(0.1f);
    }

    public void Controls()
    {
        pauseScreen.SetActive(false);
        controlScreen.SetActive(true);
    }

    public void ReturnPause()
    {
        controlScreen.SetActive(false);
        pauseScreen.SetActive(true);
    }
    #endregion

}
