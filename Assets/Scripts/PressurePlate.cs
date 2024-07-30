using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private Animator anim;
    [SerializeField] GameObject parent;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

    private void Start()
    {
        anim = GetComponent<Animator>();
        parent = transform.parent.gameObject;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // sending info to platform to move with button press and animating the button itself
    private void Update()
    {
        ButtonPress();

        //Debug.Log("pressed:" + platform.platformCalled);
    }

    //player detection for button press using raycast
    private void ButtonPress()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center,
          new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y),
          0,
          Vector2.up,
          0,
          playerLayer);

        if (hit.collider != null)
        {
           anim.SetBool("pressed", true);
        }
        else
            anim.SetBool("pressed", false);
    }

    private void OnDrawGizmos()
    {
        //if (boxCollider == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center,
          new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y));
    }
}
