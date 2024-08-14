using UnityEngine;
using UnityEngine.UI;

public class SpeedrunTimer : MonoBehaviour
{
    public Text timerText; // Assign this in the Inspector

    private void Update()
    {
        // Format the PlayTime into minutes and seconds
        int minutes = Mathf.FloorToInt(GameStateManager.PlayTime / 60F);
        int seconds = Mathf.FloorToInt(GameStateManager.PlayTime % 60F);
        int milliseconds = Mathf.FloorToInt((GameStateManager.PlayTime * 100F) % 100F);

        // Display the formatted time on the UI Text
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}
