using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    //setting to protected so the damage and collision can be checked from other scripts
    // helps with inheritance, where player and enemy are using the same logic in their code
    // specifically used here to track damage and health which is same logic for the player and enemy
    // can also be used for things like shooting projectiles if we want the player to shoot things as well
    [SerializeField] protected float damage;

    // checking collision with player and causing damage
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            collision.GetComponent<Health>().TakeDamage(damage);

    }
}
