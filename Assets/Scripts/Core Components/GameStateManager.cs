using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseScreen;

    Scene scene; // scene reference
    public static bool IsGameCompleted { get; set; } = false;    // static bool to check for game completion
    public static float PlayTime { get; private set; }     // time tracker
    public static bool Gyatt { get; set; } = true;  // allowing player to use the gun
    public static bool Paused { get; set; }    // pause tracker
    public static bool playerLost { get; set; } = false; // game over tracker
    public static bool minigameStart { get; set; } = false; // minigame tracker
    public static bool minigameEnd { get; set; } = false; // minigame tracker
    public static bool squirrelChosen { get; set; } = true; //squirrel power activation tracker

    private void Update()
    {
        if (scene.buildIndex != 0 && PauseScreen.activeInHierarchy == false)
            PlayTime += Time.deltaTime;
    }
}
