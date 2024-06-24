using UnityEngine;
using UnityEngine.UI;

public class SelectArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [Header("Audio")]
    [SerializeField] private AudioClip menuSelectSound; // play when moving
    [SerializeField] private AudioClip menuConfirmSound; // play when clicking into

    private RectTransform rect;
    private int currentPosition;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    // actual movement of the select arrow
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            ChangePosition(-1);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            ChangePosition(1);

        // interact with the options
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
            Interact();
    }

    // moving the select arrow on the y axis based on the locations of the options text objects we have
    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        // playing sound when moving the select arrow
        //if (_change != 0)
           // SoundManager.instance.PlayMenuSound(menuSelectSound);

        //preventing the position from going negative and also from going higher than our options array length
        if (currentPosition < 0)
        {
            currentPosition = options.Length - 1;
        }
        else if(currentPosition > options.Length -1)
        {
            currentPosition = 0;
        }

        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, 0);
    }

    private void Interact()
    {
        // access the button component for each option and call the function it has
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }

    // Called when an option is hovered over
    public void OnOptionHover(int index)
    {
        currentPosition = index;
        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, 0);
    }
}
