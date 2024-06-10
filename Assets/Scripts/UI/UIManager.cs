using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager manager { get; private set; }

    // referencing scene function
    Scene scene;

    // fixing issue where coming from game to menu would start cutscene again
    public static bool FromGame { get; set; }

    [Header("Main Menu")]
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject settingsScreen;

    [Header("Game Over Screen")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause Screen")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private AudioClip pauseSound;
    [SerializeField] private AudioClip menuconfirmSound;
    [SerializeField] private AudioClip sfxtestSound;

    [Header("Cutscene")]
    [SerializeField] private GameObject Cutscene;

    // [Header("Finish Screen")]
    // [SerializeField] private GameObject finishScreen;
    // [SerializeField] private AudioClip finishSound;

    private void Start()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(false);
        //menuScreen.SetActive(false);
        //Cutscene.SetActive(true);

        scene = SceneManager.GetActiveScene();

        // skip the cutscene when coming from the game through the main menu button in the game over or pause screen
        if (FromGame == true && scene.buildIndex == 0)
        {
            menuScreen.SetActive(true);
            Cutscene.SetActive(false);
        }
    }

    // allowing user to skip the intro cutscene
    // activating and deactivating pause when hitting escape, changing audio as well
    // disabling pause menu in main menu also
    private void Update()
    {
        scene = SceneManager.GetActiveScene();

        if (Input.GetKey(KeyCode.Escape) && scene.buildIndex == 0 && Cutscene.activeInHierarchy == true)
        {
            Cutscene.SetActive(false);
            menuScreen.SetActive(true);
            SoundManager.instance.SkipMainMenuMusic();
        }
        #region Pause in Menu Prevention/In Game Allowance
        // making the current scene set to the scene variable
        scene = SceneManager.GetActiveScene();

        if (scene.buildIndex != 0 && scene.buildIndex != 3)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // exit pause when hitting escape again
                if (pauseScreen.activeInHierarchy)
                {
                    SoundManager.instance.StopMusic();
                    SoundManager.instance.UnPauseMainMusic();
                    PauseGame(false);
                }
                // entering pause with escape
                else
                {
                    SoundManager.instance.PauseMainMusic();
                    SoundManager.instance.PlayPauseMusic();
                    PauseGame(true);
                }
            }
        }
        #endregion

        #region Finish Game Check

        if (GameStateManager.IsGameCompleted == true)
        {
            SoundManager.instance.StopMusic();
            //SoundManager.instance.PlayFinishMusic();
        }

        #endregion
    }

    #region Main Menu Buttons
    // main menu functions
    public void Play()
    {
        SoundManager.instance.StopMainMusic();
        menuScreen.SetActive(false);
        Cutscene.SetActive(false);
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void Settings()
    {
        menuScreen.SetActive(false);
        Cutscene.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void ReturnMainMenu()
    {
        menuScreen.SetActive(true);
        settingsScreen.SetActive(false);
    }
    #endregion

    #region Game Over Screen and Buttons
    // activate game over screen
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.StopMusic();
        SoundManager.instance.StopMainMusic();
        SoundManager.instance.PlayDeathMusic();
    }

    // game over functions
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        PauseGame(false);
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
    #endregion

}
