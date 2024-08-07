using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipButtonDoorHandler : MonoBehaviour
{
    private Animator animA;
    private Animator animB;
    private FlipButton button;
    [SerializeField] private GameObject doorA;
    [SerializeField] private GameObject doorB;
    [SerializeField] private BoxCollider2D boxColliderA;
    [SerializeField] private BoxCollider2D boxColliderB;
    [SerializeField] private AudioClip doorOpening;
    [SerializeField] private AudioClip doorClosing;
    private bool hasPlayedOpenSound = false;
    private bool hasPlayedCloseSound = false;

    //Plan

    //have two objects with the flip button, one that opens on the current state from flip button being flipped up and the other opening
    // from flipped down being true. When current state is reset, then both are closed.

    // Movement of the door objects/triggered objects tbd, right now as doors we deactive box colliders, but maybe can also implement
    //movement of walls like how the moving platforms are triggered to move to create some timed puzzles where you have to out speed the
    // door closing. Think about how to make this either possible for both cases in this script or maybe create two "door" scripts to handle
    // either case.

    private void Awake()
    {
        animA = doorA.GetComponent<Animator>();
        animB = doorB.GetComponent<Animator>();
        button = GetComponentInChildren<FlipButton>();
        boxColliderA = doorA.GetComponent<BoxCollider2D>();
        boxColliderB = doorB.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        switch (button.currentState)
        {
            case FlipButton.ButtonState.FlippedUp:
                OpenDoorA();
                CloseDoorB();
                break;
            case FlipButton.ButtonState.FlippedDown:
                OpenDoorB();
                CloseDoorA();
                break;
            case FlipButton.ButtonState.Reset:
                CloseDoorA();
                CloseDoorB();
                break;
        }
    }

    private void OpenDoorA()
    {
        boxColliderA.enabled = false;
        animA.SetBool("open", true);
        if (doorOpening != null && !hasPlayedOpenSound)
        {
            hasPlayedOpenSound = true;
            hasPlayedCloseSound = false;
        }
    }

    private void CloseDoorA()
    {
        boxColliderA.enabled = true;
        animA.SetBool("open", false);
        if (doorClosing != null && !hasPlayedCloseSound)
        {
            hasPlayedCloseSound = true;
            hasPlayedOpenSound = false;
        }
    }

    private void OpenDoorB()
    {
        boxColliderB.enabled = false;
        animB.SetBool("open", true);
        if (doorOpening != null && !hasPlayedOpenSound)
        {
            hasPlayedOpenSound = true;
            hasPlayedCloseSound = false;
        }
    }

    private void CloseDoorB()
    {
        boxColliderB.enabled = true;
        animB.SetBool("open", false);
        if (doorClosing != null && !hasPlayedCloseSound)
        {
            hasPlayedCloseSound = true;
            hasPlayedOpenSound = false;
        }
    }

    private void DoorReset()
    {
        hasPlayedCloseSound = false;
        hasPlayedOpenSound = false;
    }
}
