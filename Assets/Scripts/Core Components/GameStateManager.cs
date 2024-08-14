using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private GameObject controlScreen;

    Scene scene; // scene reference
    public static bool IsGameCompleted { get; set; } = false;    // static bool to check for game completion
    public static float PlayTime { get; private set; }     // time tracker
    public static bool Gyatt { get; set; } = false;  // allowing player to dig
    public static bool Paused { get; set; }    // pause tracker
    public static bool playerLost { get; set; } = false; // game over tracker
    public static bool minigameStart { get; set; } = false; // minigame tracker
    public static bool minigameEnd { get; set; } = false; // minigame tracker
    public static bool squirrelChosen { get; set; } = true; //squirrel power activation tracker
    public static bool Pookie{ get; set; } = true; //wolf howl activation

    private void Update()
    {
        if (scene.buildIndex != 0 && IsGameCompleted != true && playerLost != true)
        {
            if (PauseScreen.activeInHierarchy == false || controlScreen.activeInHierarchy == false)
                PlayTime += Time.deltaTime;
            else if (PauseScreen.activeInHierarchy == true || controlScreen.activeInHierarchy == true)
                PlayTime += 0;
        }

        if (IsGameCompleted == true)
            Gyatt = true;
    }

    public void TimerReset()
    {
        PlayTime = 0;
    }
}
