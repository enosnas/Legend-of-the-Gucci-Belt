using UnityEngine;
using UnityEngine.UI;

public class VolumeText : MonoBehaviour
{
    [SerializeField] private string volumeName;
    [SerializeField] private string textIntro; // will change the menu text to specified string before the volume value
    private Text txt;

    private void Awake()
    {
        txt = GetComponent<Text>();
    }

    private void Update()
    {
        UpdateVolume();
    }

    // method to change the shown text in pause menu to have volume values
    private void UpdateVolume()
    {
        // changing the value shown to be between 0-100 for user ease of use
        float volumeValue = PlayerPrefs.GetFloat(volumeName) * 100;
        // converting the stored player preference value to a string to be used in the text component
        txt.text = textIntro + volumeValue.ToString();
    }
}
