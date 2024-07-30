using UnityEngine;

public class PlayerDig : MonoBehaviour
{
    [Header("Digging Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Destructable Layer")]
    [SerializeField] private LayerMask destructLayer;

    [Header("Dig Parameters")]
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private float digCooldown;

    [Header("Audio")]
    [SerializeField] private AudioClip digSound;

    //reference to health of the destructable object
    private DestructableHealth destructHealth;
    private Animator anim;
    private Health health;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // letting the player attack when left clicking
    private void Update()
    {
        if(GameStateManager.Gyatt == true)
        {
            cooldownTimer += Time.deltaTime;

            if (Input.GetMouseButtonDown(0) && cooldownTimer >= digCooldown && canDig() && GameStateManager.Paused != true)
            {
                GopherDig();
                SoundManager.instance.PlaySound(digSound);
            }
            else
            {
                StopDigging();
                cooldownTimer += Time.deltaTime;
            }
            //Debug.Log("Dug:" + cooldownTimer);
        }
    }

    #region Digging RayCast
    private bool DestructionDetect()
    {
        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePosition.z = transform.position.z; // Ensure the z-coordinate is zero, as we are working in 2D

        // Calculate the direction from the player to the mouse
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Perform the BoxCast in the direction of the mouse
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + (Vector3)(direction * range *  colliderDistance),
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0,
            direction,
            0,
            destructLayer
        );

        if (hit.collider != null)
            destructHealth = hit.transform.GetComponent<DestructableHealth>();


        return hit.collider != null;
    }


    //visualizing the dig range of the player within Unity using gizmos
    private void OnDrawGizmos()
    {
        if (boxCollider == null) return;

        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure the z-coordinate is zero, as we are working in 2D

        // Calculate the direction from the player to the mouse
        Vector2 direction = (mousePosition - transform.position).normalized;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + (Vector3)(direction * range *  colliderDistance),
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        );
    }


    #endregion

    // damaging the object in range
    private void GopherDig()
    {
        if (DestructionDetect() && destructHealth != null)
                destructHealth.TakeDamage(damage);

        anim.SetTrigger("attacking");
        cooldownTimer = 0;
    }

    // logic to allow attacking and to attack and reset cooldown timer
    public bool canDig()
    {
        return cooldownTimer >= digCooldown && health.dead == false;
    }

    //setting animation back to false for use in animation event
    // still having issues with jump overriding the attack animation and reference here when you come back to fix (fix the bools)
    private void StopDigging()
    {
        anim.SetBool("attacking", false);
        anim.SetBool("attackingUp", false);
        anim.SetBool("attackingDown", false);
    }
}
