using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseScreen;

    Scene scene;
    
    // static bool to check for game completion
    public static bool IsGameCompleted { get; set; } = false;

    // time tracker
    public static float PlayTime { get; private set; }

    // allowing player to use the gun
    public static bool Gyatt { get; set; }

    public static bool Paused { get; set; }

    public static bool playerLost { get; set; } = false;

    private void Update()
    {
        if (scene.buildIndex != 0 && PauseScreen.activeInHierarchy == false)
            PlayTime += Time.deltaTime;
    }
}
