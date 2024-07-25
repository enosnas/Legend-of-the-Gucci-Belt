using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearObjectMovement : MonoBehaviour
{
    [SerializeField ]private float fearResetTime;
    [SerializeField] private float speed;

    private FearLevel fearLevel;
    private BoxCollider2D boxCollider;
    private Rigidbody2D body;
    private float fearLifetime;
    private bool hit;


    private void Start()
    {
        fearLevel = GetComponent<FearLevel>();
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (hit) return;

        if (fearLevel.feared == true)
        {
            body.isKinematic = false;

            transform.localScale = new Vector3(6, 6, 1);

            float movementSpeed = speed * Time.deltaTime * transform.localScale.x;
            transform.Translate(movementSpeed, 0, 0);

            fearLifetime += Time.deltaTime;

            if (fearLifetime > fearResetTime)
                gameObject.SetActive(false);
        }
        else
        {
            body.isKinematic = true;

            transform.localScale = new Vector3(-6, 6, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "Ground")
        {
            body.velocity = Vector2.zero;
        }
        else if(collision.tag == "Despawner")
        {
            hit = true;
            boxCollider.enabled = false;
            gameObject.SetActive(false);
        }
    }
}
