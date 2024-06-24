using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private GameObject[] bullets;
    private Animator anim;
    private Health health;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Audio")]
    [SerializeField] private AudioClip pistolSound;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    // letting the player attack when left clicking
    private void Update()
    {
        if(GameStateManager.Gyatt == true)
        {
            if (Input.GetMouseButtonDown(0) && cooldownTimer > attackCooldown && canAttack())
            {
                Attack();
            }
            else
            {
                anim.SetBool("attacking", false);
                cooldownTimer += Time.deltaTime;
            }
            //Debug.Log("Attacked:" + cooldownTimer);
        }
    }

    // logic to allow attacking and to attack and reset cooldown timer
    public bool canAttack()
    {
        return cooldownTimer > attackCooldown && health.dead == false;
    }

    // player attack cooldown reset and animation
    private void Attack()
    {
        SoundManager.instance.PlaySound(pistolSound);
        anim.SetBool("attacking", true);
        cooldownTimer = 0;

        bullets[FindBullet()].transform.position = gunPoint.position;
        bullets[FindBullet()].GetComponent<Bullet>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    //setting animation back to false for use in animation event
    // still having issues with jump overriding the attack animation and reference here when you come back to fix (fix the bools)
    private void StopAttacking()
    {
        anim.SetBool("attacking", false);
    }

    // bullet pooling
    private int FindBullet()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
