using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMovementPlatformHorizontal : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform pointLeft;
    [SerializeField] private Transform pointRight;
    private Vector2 pointLeftInitialPos;
    private Vector2 pointRightInitialPos;
    private Vector2 currentPointPos;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    public bool platformCalled = false;
    public bool buttonReturn;
    public float directionM;
    public float speedAdj;

    // setting movement positions for the platform
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Store initial positions
        pointLeftInitialPos = pointLeft.position;
        pointRightInitialPos = pointRight.position;
        currentPointPos = pointLeftInitialPos;
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
        Vector2 directionD = (currentPointPos - (Vector2)transform.position).normalized;
        body.velocity = new Vector2(directionD.x * speed, body.velocity.y);
        //transform.Translate(direction.x * speed * Time.deltaTime, 0, 0);
        //transform.position = Vector3.MoveTowards(transform.position, currentPointPos, speed * Time.deltaTime);
        directionM = Mathf.Sign(directionD.x);
        speedAdj = speed * directionM;
    }

    private void CheckDirection()
    {
        if (Vector2.Distance(transform.position, currentPointPos) < 0.5f)
        {
            platformCalled = false;
            body.velocity = Vector2.zero;
            buttonReturn = true;

            if (currentPointPos == pointLeftInitialPos)
            {
                currentPointPos = pointRightInitialPos;
            }
            else
            {
                currentPointPos = pointLeftInitialPos;
            }
        }
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

        Gizmos.DrawWireCube(pointRight.position, colliderSize);
        Gizmos.DrawWireCube(pointLeft.position, colliderSize);
        Gizmos.DrawLine(pointRight.position, pointLeft.position);
        //Gizmos.DrawWireCube(pointRight.position, new Vector3(4, 0.1f, 0));
    }
}