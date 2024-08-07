using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipButton : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float range;

    public enum ButtonState { Reset, FlippedUp, FlippedDown }
    public ButtonState currentState = ButtonState.Reset;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        ButtonFlip();
    }

    private void ButtonFlip()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center,
            new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y * range),
            0,
            Vector2.left,
            0,
            playerLayer);

        if (hit.collider != null && Input.GetKeyDown(KeyCode.E))
        {
            switch (currentState)
            {
                case ButtonState.Reset:
                    anim.SetBool("flippedUp", true);
                    anim.SetBool("flippedDown", false);
                    anim.SetBool("reset", false);
                    currentState = ButtonState.FlippedUp;
                    break;

                case ButtonState.FlippedUp:
                    anim.SetBool("flippedUp", false);
                    anim.SetBool("flippedDown", true);
                    anim.SetBool("reset", false);
                    currentState = ButtonState.FlippedDown;
                    break;

                case ButtonState.FlippedDown:
                    anim.SetBool("flippedUp", false);
                    anim.SetBool("flippedDown", false);
                    anim.SetBool("reset", true);
                    currentState = ButtonState.Reset;
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxCollider.bounds.center,
            new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y * range));
    }
}
