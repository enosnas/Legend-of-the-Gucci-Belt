using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateDoor : MonoBehaviour
{
    private Animator anim;
    private PressurePlate plate;
    [SerializeField] GameObject parent;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] AudioClip doorOpening;
    [SerializeField] AudioClip doorClosing;
    private bool hasPlayedOpenSound = false;
    private bool hasPlayedCloseSound = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        parent = transform.parent.gameObject;
        plate = parent.GetComponentInChildren<PressurePlate>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (plate.doorOpen)
        {
            boxCollider.enabled = false;
            anim.SetBool("open", true);
            if (doorOpening != null && !hasPlayedOpenSound)
            {
                SoundManager.instance.PlaySound(doorOpening);
                hasPlayedOpenSound = true;
                hasPlayedCloseSound = false; // Reset the close sound flag
            }
        }
        else if (plate.doorClose)
        {
            boxCollider.enabled = true;
            anim.SetBool("open", false);
            if (doorClosing != null && !hasPlayedCloseSound)
            {
                SoundManager.instance.PlaySound(doorClosing);
                hasPlayedCloseSound = true;
                hasPlayedOpenSound = false; // Reset the open sound flag
            }
        }
    }
}
