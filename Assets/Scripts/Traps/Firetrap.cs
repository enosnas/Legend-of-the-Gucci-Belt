using System.Collections;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header ("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;
    private Health playerHealth;

    [Header("Audio")]
    [SerializeField] private AudioClip firetrapSound;
    [SerializeField] private float volume;

    private bool triggered;
    private bool active;

    private void Awake()
    {
        
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    // allowing for updates to collision to fix issue where you dont take damage if you stay
    // on the trap
    private void Update()
    {
        if(playerHealth != null && active)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    // trigger the fire trap on collision, get the player health, and deal damage
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<Health>();
            
            if(!triggered)
            {
                StartCoroutine(ActivateFiretrap());
            }
            if(active)
                collision.GetComponent<Health>().TakeDamage(damage);
            
        }
    }

    // this assumes that the player health is not null since the player is on the trap and
    // once the player exits the trap collider it sets it to null, works with damage above and
    // making sure we dont damage player if they arent on the trap when it activates
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth = null;
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        // activating the trap, turning the sprite red for a sec to visualize the activation
        // then change back to white and set to active before deactivating based on our variables
        triggered = true;
        spriteRend.color = Color.red;

        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(firetrapSound);
        spriteRend.color = Color.white;
        active = true;
        anim.SetBool("activated", true);

        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}
