using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class FearLevel : MonoBehaviour
{
    [Header("Sanity Level Parameters")]
    [SerializeField] private float startingSanity;
    public float currentSanity { get; private set; }
    private Animator anim;
    public bool Feared { get; private set; }

    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    [Header("Audio")]
    [SerializeField] private AudioClip afraidSound;
    [SerializeField] private AudioClip fearedSound;

    // starting off with the maximum health, and changing the max health based on the level the player is in
    private void Awake()
    {
        currentSanity = startingSanity;

        anim = GetComponent<Animator>();
    }

    // the calculation for damage
    public void TakeSanityDamage(float _damage)
    {
        currentSanity = Mathf.Clamp(currentSanity - _damage, 0, startingSanity);

        if (currentSanity > 0)
        {
            anim.SetTrigger("afraid");
            SoundManager.instance.PlaySound(afraidSound);
        }
        else
        {
            if (!Feared)
            {
                anim.SetTrigger("feared");

                // disabling selected items when feared, such as box collider
                if(components != null)
                {
                    foreach (Behaviour component in components)
                        component.enabled = false;
                }

                Feared = true;
                SoundManager.instance.PlaySound(fearedSound);
            }

        }
    }
}
