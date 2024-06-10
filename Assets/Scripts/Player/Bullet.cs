using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float bulletResetTime;
    [SerializeField] private float damage;
    private float direction;
    private bool hit;
    private float bulletLifetime;

    private BoxCollider2D boxCollider;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // if the bullet hits something it will just return and not execute the rest of the code
    // if not then it will be moved by the speed on the x axis as defined above
    // also setting time to expire on the bullets and deactivating if they hit expiry
    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        bulletLifetime += Time.deltaTime;
        if (bulletLifetime > bulletResetTime)
            gameObject.SetActive(false);
    }

    // checking collision of bullet with other objects and deactivating if true
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        gameObject.SetActive(false);

        if (collision.tag == "Enemy")
            collision.GetComponent<Health>().TakeDamage(damage);

    }

    // use to tell the bullet which direction to shoot and also reset the state of the bullet
    public void SetDirection(float _direction)
    {
        bulletLifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;
    }
}
