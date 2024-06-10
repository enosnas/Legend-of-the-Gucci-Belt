using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows;
    private float cooldownTimer;

    [Header("Audio")]
    [SerializeField] private AudioClip arrowSound;

    //sets the cooldown to 0 and will count up until it can attack again as set later in update
    // also resets the position of the arrows to the original position and then shoots in the
    // correct direction
    private void Attack()
    {
        cooldownTimer = 0;
        SoundManager.instance.PlaySound(arrowSound);
        arrows[FindArrow()].transform.position = firePoint.position;
        arrows[FindArrow()].GetComponent<ArrowProjectile>().ActivateProjectile();
    }

    // allows us to pool arrows instead of creating infinite, we will have a set that are usable
    private int FindArrow()
    {
        for (int i = 0; i< arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown)
            Attack();
    }
}
