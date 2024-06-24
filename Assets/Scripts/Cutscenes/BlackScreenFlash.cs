using UnityEngine;
using UnityEngine.UI;

public class BlackScreenFlash : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private GameObject menuScreen;

    private float changeColorTime = 21.62f; // Time to change to white
    private float fadeDuration = 10.42f; // Duration of the fade out
    private Image image;
    private CanvasGroup canvasGroup;
    public static bool isWhite { get; set; } = false;
    private float timer = 0f;

    void Start()
    {
        image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Start with a fully opaque black image
        image.color = Color.black;
        canvasGroup.alpha = 1f;
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changeColorTime && !isWhite)
        {
            // Change to white instantly at the specified time
            image.color = Color.white;
            isWhite = true;
            menuScreen.SetActive(true);
            timer = 0f; // Reset timer to start fade out immediately
        }

        if (isWhite)
        {
            // Start fading out
            float fadeRatio = timer / fadeDuration;
            canvasGroup.alpha = 1f - fadeRatio;

            // Destroy the object or disable the script once fully faded out
            if (fadeRatio >= 1f)
            {
                canvasGroup.alpha = 0f;
                // Optionally disable the object or the script
                gameObject.SetActive(false);
                // Alternatively, if you want to destroy the object:
                // Destroy(gameObject);
            }
        }
    }
}
