using UnityEngine;

public class PlayerHowl : MonoBehaviour
{
    [Header("Howl Collider Parameters")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float range;


    [Header("Enemy Layer")]
    [SerializeField] private LayerMask enemyLayer;

    [Header("Howl Parameters")]
    [SerializeField] private float damage;
    [SerializeField] private float howlCooldown;

    [Header("Audio")]
    [SerializeField] private AudioClip howlSound;

    private Animator anim;
    private Health health;
    private FearLevel fearLevel;
    private PlayerMovement playerMove;
    private float cooldownTimer = Mathf.Infinity;
    private float howlTimer = Mathf.Infinity;
    private float howlDuration = 4.672f;
    public bool howling;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerMove = GetComponent<PlayerMovement>();
    }

    // letting the player attack when left clicking
    private void Update()
    {
        if (GameStateManager.Pookie == true)
        {
            cooldownTimer += Time.deltaTime;
            howlTimer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.LeftShift) && cooldownTimer >= howlCooldown && canHowl())
            {
                anim.SetTrigger("howl");
                PookieHowl();
                howling = true;
                SoundManager.instance.PlaySound(howlSound);
                cooldownTimer = 0;
                howlTimer = 0;
            }

            if(howlTimer >= howlDuration)
            {
                anim.SetBool("howl", false);
                howling = false;
                cooldownTimer += Time.deltaTime;
            }
            //Debug.Log("Dug:" + cooldownTimer);
        }
    }

    #region Digging RayCast
    private bool HowlDetect()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center,
           new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y * range/2),
           0,
           Vector2.left,
           0,
           enemyLayer);

        if (hit.collider != null)
            fearLevel = hit.transform.GetComponent<FearLevel>();


        return hit.collider != null;
    }


    //visualizing the dig range of the player within Unity using gizmos
    private void OnDrawGizmos()
    {
        if (boxCollider == null) return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(boxCollider.bounds.center,
           new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y * range/2));
    }


    #endregion

    // damaging the object in range
    private void PookieHowl()
    {
        if (HowlDetect() && fearLevel != null)
            fearLevel.TakeSanityDamage(damage);
    }

    // logic to allow attacking and to attack and reset cooldown timer
    public bool canHowl()
    {
        return cooldownTimer >= howlCooldown && health.dead == false && playerMove.isGrounded() == true && playerMove.isSwimming() == false;
    }

    //setting animation back to false for use in animation event
    // still having issues with jump overriding the attack animation and reference here when you come back to fix (fix the bools)
    private void StopHowling()
    {
        anim.SetBool("howl", false);
    }
}
