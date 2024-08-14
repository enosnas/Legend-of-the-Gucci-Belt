using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButtonDoor : MonoBehaviour
{
    private Animator anim;
    private PressureButton button;
    [SerializeField] GameObject parent;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] AudioClip doorOpening;
    [SerializeField] AudioClip doorClosing;
    private bool hasPlayedOpenSound = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        parent = transform.parent.gameObject;
        button = parent.GetComponentInChildren<PressureButton>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (button.doorOpen)
        {
            boxCollider.enabled = false;
            anim.SetBool("open", true);
            if (doorOpening != null && !hasPlayedOpenSound)
            {
                SoundManager.instance.PlaySound(doorOpening);
                hasPlayedOpenSound = true;
            }
        }
    }
}
