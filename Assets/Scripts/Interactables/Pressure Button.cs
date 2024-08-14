using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private BoxCollider2D boxCollider;
    public bool doorOpen;

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
        }
        else if (collision.CompareTag("Player") && collision.CompareTag("Wildlife"))
            return;
    }
}
