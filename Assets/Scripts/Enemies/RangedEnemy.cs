using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Ranged Attack")]
    [SerializeField] private Transform gunpoint;
    [SerializeField] private GameObject[] bullets;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Audio")]
    [SerializeField] private AudioClip pistolSound;

    private float cooldownTimer = Mathf.Infinity;


    // references for animation and enemy patrol object, also references to player and player health
    private Animator anim;
    private EnemyPatrol enemyPatrol;
    private Health playerHealth;
    private Transform player;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        // getting the script from enemypatrol for our reference in making patrol stop when player is seen
        // using get from parent as the enemy object is a child of enemy patrol object
        enemyPatrol = GetComponentInParent<EnemyPatrol>();

        // getting reference to the player and getting the health component 
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
        }
    }
    private void Update()
    {
        // need to work on stopping the attack when player health is 0 but right now referencing the ranged enemy hp and not player's
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            //Attack only when the player is in sight and their hp is greater than 0
            if (cooldownTimer >= attackCooldown && playerHealth.currentHealth > 0)
            {
                cooldownTimer = 0;
                anim.SetTrigger("rangedAttack");

            }
        }

        // if we see the player while patrolling then we stop patrolling
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }
    }

    // reseting cooldown after each shot and also setting location to shoot from for enemy
    private void RangedAttack()
    {
        SoundManager.instance.PlaySound(pistolSound);
        cooldownTimer = 0;
        bullets[FindBullets()].transform.position = gunpoint.position;
        bullets[FindBullets()].GetComponent<EnemyProjectile>().ActivateEnemyProjectile();
    }

    // fireball pooling for the enemy projectile
    private int FindBullets()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private bool PlayerInSight()
    {
        // box raycast to check if the player is within sight
        // using a vector 3 so that we can change the size of the box in terms of length/x
        // also added in colliderDistance so we can adjust how far the hitbox starts from the enemy
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    //visualizing the sight of the enemy within Unity using gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
