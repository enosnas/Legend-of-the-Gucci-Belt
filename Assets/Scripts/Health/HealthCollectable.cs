using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    // how much health to restore
    [SerializeField] private float healthValue;

    [Header("Audio")]
    [SerializeField] private AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag =="Player")
        {
            SoundManager.instance.PlaySound(collectSound);
            collision.GetComponent<Health>().AddHealth(healthValue);
            gameObject.SetActive(false);
        }
    }
}
