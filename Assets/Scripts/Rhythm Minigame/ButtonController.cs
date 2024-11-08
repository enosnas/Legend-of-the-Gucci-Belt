using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // assigning images for pressed and unpressed buttons
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private Sprite pressedImage;

    [SerializeField] private KeyCode keytoPress;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // changing the sprite when the player presses and releases the button
    private void Update()
    {
        if (Input.GetKeyDown(keytoPress))
            spriteRenderer.sprite = pressedImage;

        if (Input.GetKeyUp(keytoPress))
            spriteRenderer.sprite = defaultImage;
    }

}
