using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMovementPlatform : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform pointDown;
    [SerializeField] private Transform pointUp;
    private Vector2 pointDownInitialPos;
    private Vector2 pointUpInitialPos;
    private Vector2 currentPointPos;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    public bool platformCalled = false;
    public bool buttonReturn;
    private Rigidbody2D playerBody;

    // setting movement positions for the platform
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        // Store initial positions
        pointDownInitialPos = pointDown.position;
        pointUpInitialPos = pointUp.position;
        currentPointPos = pointDownInitialPos;
    }

    private void FixedUpdate()
    {
        if (platformCalled == true)
        {
            MovePlatform();
            CheckDirection();
        }
    }

    #region Platform movement
    private void MovePlatform()
    {
            Vector2 direction = (currentPointPos - (Vector2)transform.position).normalized;
            body.velocity = new Vector2(body.velocity.x, direction.y * speed);
    }

    private void CheckDirection()
    {
        if (Vector2.Distance(transform.position, currentPointPos) < 0.01f)
        {
            platformCalled = false;
            body.velocity = Vector2.zero;
            buttonReturn = true;

            if (currentPointPos == pointDownInitialPos)
            {
                currentPointPos = pointUpInitialPos;
            }
            else
            {
                currentPointPos = pointDownInitialPos;
            }
        }
    }
    #endregion

    #region Adjusting Player Gravity

    // added as the player will slightly jump up once the vertical platforms reach the positionUp location and this prevents that jump
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerBody.gravityScale += 7.9f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerBody.gravityScale -= 7.9f;
    }
    #endregion

    //Flow

    // player presses trigger -> platform moves to location -> platform stops -> platform changes desired location to opposite ->
    // platform waits for trigger to move again

    private void OnDrawGizmos()
    {
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }

        Vector3 colliderSize = new Vector3(boxCollider.size.x+2.5f, boxCollider.size.y, 1f);

        Gizmos.DrawWireCube(pointUp.position, colliderSize);
        Gizmos.DrawWireCube(pointDown.position, colliderSize);
        Gizmos.DrawLine(pointUp.position, pointDown.position);
        //Gizmos.DrawWireCube(pointUp.position, new Vector3(4, 0.1f, 0));
    }
}