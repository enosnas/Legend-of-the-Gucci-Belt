using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip checkpointSound;

    private Transform currentCheckpoint;
    private Health playerHealth;
    private PlayerUIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<PlayerUIManager>();
    }

    private void Update()
    {
        Debug.Log(currentCheckpoint);
    }

    public void CheckRespawn()
    {
        // checking if there is a current checkpoint
        if (currentCheckpoint == null)
        {
            //show game over screen
            uiManager.GameOver();
            GameStateManager.playerLost = true;
            return;
        }
        else
        {
            // moving the player to the checkpoint position
            transform.position = currentCheckpoint.position;

            // restore the player health and reset animation
            playerHealth.Respawn();

            // Restart the level music
            //SoundManager.instance.PlayMainMusic();

        }
    }

    // stop music when player dies
    public void PlayerDied()
    {
        // Stop the level music
        SoundManager.instance.StopMusic();
    }

    // checkpoint activation
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag =="Checkpoint")
        {
            // storing the checkpoint we just collided with as the new current checkpoint
            currentCheckpoint = collision.transform;

            SoundManager.instance.PlaySound(checkpointSound);

            // deactivating the checkpoint collider after hitting it once
            collision.GetComponent<Collider2D>().enabled = false;
            //starting animation
            collision.GetComponent<Animator>().SetTrigger("appear");
        }
    }
}
