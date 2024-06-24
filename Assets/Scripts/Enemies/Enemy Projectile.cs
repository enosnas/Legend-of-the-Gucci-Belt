using UnityEngine;

public class EnemyProjectile : EnemyDamage // adds inheritance from the enemy damage script
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private Animator anim;
    private bool hit;
    private BoxCollider2D coll;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    // activating the enemy projectiles and giving it a lifetime before expiring and reseting
    public void ActivateEnemyProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;
    }

    // setting the movement speed of the enemy projectile and setting it to deactivate after it reaches lifetime
    private void Update()
    {
        // in case there is a collision then do not continue to move
        if (hit) return;

        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed,0,0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        hit = true;

        //run the logic from the parent script enemydamage which we inherit from first then ours
        // happens since we have ontriggerenter2d in the inheritance and here as well
        base.OnTriggerEnter2D(collision);

        // disabling the collider as well once collision occurs
        coll.enabled = false;
        
        //when hitting an object deactivate
        gameObject.SetActive(false);

        // when the bullet collides make it explode
      //  anim.SetTrigger("explode");
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
