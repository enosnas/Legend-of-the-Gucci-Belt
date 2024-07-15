using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DestructableHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    public bool destroyed { get; private set; }

    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    [Header("Audio")]
    [SerializeField] private AudioClip destroyedSound;
    [SerializeField] private AudioClip damagedSound;

    // starting off with the maximum health, and changing the max health based on the level the player is in
    private void Awake()
    {
        currentHealth = startingHealth;

        anim = GetComponent<Animator>();
    }

    // the calculation for damage
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("damaged");
            SoundManager.instance.PlaySound(damagedSound);
        }
        else
        {
            if (!destroyed)
            {
                // disabling all items with the health script upon their death
                foreach (Behaviour component in components)
                    component.enabled = false;

                anim.SetTrigger("destroyed");

                destroyed = true;
                SoundManager.instance.PlaySound(destroyedSound);
            }

        }
    }
}
