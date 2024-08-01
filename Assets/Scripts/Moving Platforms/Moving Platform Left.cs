using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformLeft : MonoBehaviour
{
    [SerializeField] private float platformResetTime;
    [SerializeField] public float speed;
    private float platformLifetime;
    private bool hit;
    private bool activated;
    private BoxCollider2D boxCollider;
    private Rigidbody2D body;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (hit) return;

        if (activated == true)
        {
            float platformSpeed = speed * Time.deltaTime;
            transform.Translate(platformSpeed, 0, 0);

            platformLifetime += Time.deltaTime;
        }

        if (platformLifetime > platformResetTime)
            DeactivatePlatform();
    }

    public void InitializePlatform()
    {
        platformLifetime = 0;
        gameObject.SetActive(false);
        if(boxCollider != null)
            boxCollider.enabled = true;
        hit = false;
        activated = false;
    }

    public void ActivatePlatform()
    {
        platformLifetime = 0;
        gameObject.SetActive(true);
        if(boxCollider != null)
            boxCollider.enabled = true;
        hit = false;
        activated = true;
    }

    public void DeactivatePlatform()
    {
        platformLifetime = 0;
        gameObject.SetActive(false);
        if (boxCollider != null)
            boxCollider.enabled = false;
        hit = true;
        activated = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Despawner")
        {
            DeactivatePlatform();
        }
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.transform.parent = transform;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.transform.parent = null;
    }*/
}
