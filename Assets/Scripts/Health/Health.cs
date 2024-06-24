using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Health : MonoBehaviour
{   
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    public bool dead { get; private set; }

    Scene scene;
   
    [Header("iFrames")]
    [SerializeField] private float iFrameDuration;
    [SerializeField] private int numberofFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    [Header("Audio")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    // starting off with the maximum health, and changing the max health based on the level the player is in
    private void Awake()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.buildIndex == 2)
            startingHealth += 1;
        else if(scene.buildIndex == 3)
            startingHealth += 3;

        currentHealth = startingHealth;

        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    // the calculation for damage
    public void TakeDamage(float _damage)
    {
        if(invulnerable) return;

        currentHealth = Mathf.Clamp(currentHealth - _damage,0,startingHealth);

        if (currentHealth > 0 )
        {
            anim.SetTrigger("hurt");
            // cant call ienumerators like normal methods and have to use this to call it
            StartCoroutine(Invulnerability());
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            if (!dead)
            {
                // disabling all items with the health script upon their death
                foreach (Behaviour component in components)
                    component.enabled = false;

                anim.SetBool("grounded", true);
                anim.SetTrigger("die");

                dead = true;
                SoundManager.instance.PlaySound(deathSound);
            }
            
        }
    }

    // adding player respawn and reseting the death animation and setting to idle and reactivating components
    public void Respawn()
    {
        dead = false;
        AddHealth(startingHealth);
        anim.ResetTrigger("die");
        anim.Play("idle");
        StartCoroutine(Invulnerability());

        // enabling all items with the health script upon respawn
        foreach (Behaviour component in components)
            component.enabled = true;
    }

    // health collectable health adding
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    // creating iFrames
    private IEnumerator Invulnerability()
    {
        invulnerable = true;
        //setting the player to ignore collisions on the enemy layer 9
        Physics2D.IgnoreLayerCollision(8,9,true);
        // invulnerability duration set with a for loop to last as long as we set number of flashes to
        // will also set the color of our player to a light red flashing and then wait before changing us back to the default white
        for (int i = 0; i < numberofFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFrameDuration / (numberofFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFrameDuration / (numberofFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(8, 9, false);
        invulnerable = false;
    }

}
