using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButton : MonoBehaviour
{
    private Animator anim;
    private SetMovementPlatform platform;
    [SerializeField] GameObject parent;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float range;
    [SerializeField] private LayerMask playerLayer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        parent = transform.parent.gameObject;
        platform = parent.GetComponentInChildren<SetMovementPlatform>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // sending info to platform to move with button press and animating the button itself
    private void Update()
    {
        ButtonPress();

        if (platform.buttonReturn == true)
            anim.SetBool("pressed",false);

        //Debug.Log("pressed:" + platform.platformCalled);
    }

    //player detection for button press using raycast
    private void ButtonPress()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center,
          new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y * range / 2),
          0,
          Vector2.left,
          0,
          playerLayer);

        if(hit.collider != null)
        {
            if (Input.GetKey(KeyCode.E) && platform.platformCalled == false)
            {
                platform.buttonReturn = false;
                anim.SetBool("pressed", true);
                platform.platformCalled = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        //if (boxCollider == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center,
          new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y * range / 2));
    }
}
