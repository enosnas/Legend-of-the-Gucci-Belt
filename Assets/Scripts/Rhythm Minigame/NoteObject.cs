using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    private bool canbePressed;
    public KeyCode keytoPress;

    private void Update()
    {
        if (Input.GetKeyDown(keytoPress))
            if (canbePressed)
                gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Rhythm Button")
            canbePressed = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Rhythm Button")
            canbePressed = false;
    }
}
