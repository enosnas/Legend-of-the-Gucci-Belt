using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private BoxCollider2D boxCollider;
    //[SerializeField] private LayerMask pressureLayer;
    [SerializeField] AudioClip doorOpening;
    [SerializeField] AudioClip doorClosing;
    public bool doorOpen;
    public bool doorClose;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // sending info to platform to move with button press and animating the button itself
    private void Update()
    {
        //ButtonPress();

        //Debug.Log("pressed:" + platform.platformCalled);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Wildlife"))
        {
            anim.SetBool("pressed", true);
            doorOpen = true;
            SoundManager.instance.PlaySound(doorOpening);
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
            SoundManager.instance.PlaySound(doorClosing);
        }
        else if (collision.CompareTag("Player") && collision.CompareTag("Wildlife"))
            return;
    }

    /*player detection for button press using raycast
    private void ButtonPress()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center,
          new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y),
          0,
          Vector2.up,
          0,
          pressureLayer);

        if (hit.collider != null)
        {
           anim.SetBool("pressed",true);
            doorOpen = true;
        }
        else
        {
            anim.SetBool("pressed",false);
            doorOpen = false;
        }
    }
    
    private void OnDrawGizmos()
    {
        //if (boxCollider == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center,
          new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y));
    }*/
}
