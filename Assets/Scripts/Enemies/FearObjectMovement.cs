using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearObjectMovement : MonoBehaviour
{
    [SerializeField] private float fearResetTime;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask pressureLayer;

    private float pressurePos;
    private float snapSpeed = 2f;
    private bool snappingtoPlate;

    private FearLevel fearLevel;
    private BoxCollider2D boxCollider;
    private Rigidbody2D body;
    private Animator anim;
    private float fearLifetime;
    private bool hit;

    private void Awake()
    {
        fearLevel = GetComponent<FearLevel>();
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (hit) return;

        // Making the object run to the right until it hits a despawner, runs out of lifetime, or lands on a pressure plate
        if (fearLevel.afraid)
        {
            if (ontoPlate())
            {
                body.velocity = Vector2.zero;
                body.isKinematic = true;
                anim.SetBool("running", false);
                anim.SetBool("afraid", true);

                if (snappingtoPlate)
                {
                    Vector3 targetPosition = new Vector3(pressurePos, transform.position.y, transform.position.z);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * snapSpeed);

                    if (Mathf.Abs(transform.position.x - pressurePos) < 0.01f)
                    {
                        transform.position = targetPosition;
                        snappingtoPlate = false;
                    }
                }
            }
            else
            {
                body.isKinematic = false;
                anim.SetBool("running", true);
                SetDirection(fearLevel.howlDirection);

                float movementSpeed = speed * transform.localScale.x;
                body.velocity = new Vector2(movementSpeed, body.velocity.y);
                //transform.Translate(movementSpeed, body.velocity.y, 0);
            }
        }
        else if (fearLevel.feared)
        {
            anim.SetBool("afraid", false);
            anim.SetBool("running", true);

            body.isKinematic = false;
            SetDirection(fearLevel.howlDirection);

            float movementSpeed = speed * transform.localScale.x;
            body.velocity = new Vector2(movementSpeed, body.velocity.y);
            //transform.Translate(movementSpeed, body.velocity.y, 0);

            fearLifetime += Time.deltaTime;

            if (fearLifetime > fearResetTime)
                gameObject.SetActive(false);
        }
        else // Making the object unmovable unless feared and also face left
        {
            body.isKinematic = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Ground")
        {
            body.velocity = Vector2.zero;
        }
        else if (collision.tag == "Despawner")
        {
            hit = true;
            boxCollider.enabled = false;
            gameObject.SetActive(false);
        }
        else if (collision.tag == "Pressure Plate")
        {
            snappingtoPlate = true;
        }
    }

    private bool ontoPlate()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            Vector2.down,
            0.1f,
            pressureLayer);

        if (raycastHit.collider != null)
            pressurePos = raycastHit.transform.position.x;

        return raycastHit.collider != null;
    }

    private void SetDirection(int howlDirection)
    {
        // Ensure that the object faces the correct direction
        float scaleX = Mathf.Abs(transform.localScale.x) * -howlDirection;
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
    }
}
