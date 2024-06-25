using UnityEngine;

public class ArrowProjectile : EnemyDamage // adds inheritance from the enemy damage script
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;

    // activating the arrow projectiles and giving it a lifetime before expiring and reseting
    public void ActivateProjectile()
    {
        lifetime = 0;
        gameObject.SetActive(true);
    }

    // setting the movement speed of the arrow and setting it to deactivate after it reaches lifetime
    private void Update()
    {
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed,0,0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //run the logic from the parent script enemydamage which we inherit from first then ours
        // happens since we have ontriggerenter2d in the inheritance and here as well
        base.OnTriggerEnter2D(collision);

        //when hitting an object deactivate
        gameObject.SetActive(false);
    }
}
