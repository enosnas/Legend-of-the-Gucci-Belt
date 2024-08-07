using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private BoxCollider2D boxCollider;
    public bool doorOpen;
    public bool doorClose;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Wildlife"))
        {
            anim.SetBool("pressed", true);
            doorOpen = true;
            doorClose = false;

        }
        else if (collision.CompareTag("Player") && collision.CompareTag("Wildlife"))
            return;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Wildlife"))
        {
            anim.SetBool("pressed", false);
            doorOpen = false;
            doorClose = true;

        }
        else if (collision.CompareTag("Player") && collision.CompareTag("Wildlife"))
            return;
    }
}
