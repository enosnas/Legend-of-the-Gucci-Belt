using UnityEngine;

public class PlayerAttack : MonoBehaviour
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
            if (Input.GetMouseButtonDown(0) && cooldownTimer > digCooldown && canDig())
            {
                GopherDig();
                anim.SetTrigger("attacking");
                SoundManager.instance.PlaySound(digSound);
            }
            else
            {
                anim.SetBool("attacking", false);
                cooldownTimer += Time.deltaTime;
            }
            //Debug.Log("Attacked:" + cooldownTimer);
        }
    }

    #region Digging RayCast
    private bool DestructionDetect()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, destructLayer);

        if (hit.collider != null)
            destructHealth = hit.transform.GetComponent<DestructableHealth>();

        return hit.collider != null;
    }

    //visualizing the dig range of the player within Unity using gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    #endregion

    // damaging the object in range
    private void GopherDig()
    {
        if (DestructionDetect())
        {
            destructHealth.TakeDamage(damage);
        }
    }

    // logic to allow attacking and to attack and reset cooldown timer
    public bool canDig()
    {
        return cooldownTimer > digCooldown && health.dead == false;
    }

    //setting animation back to false for use in animation event
    // still having issues with jump overriding the attack animation and reference here when you come back to fix (fix the bools)
    private void StopDigging()
    {
        anim.SetBool("attacking", false);
    }
}
