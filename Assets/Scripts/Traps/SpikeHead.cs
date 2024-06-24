using UnityEngine;

public class SpikeHead : EnemyDamage
{
    [Header("Spikehead Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private float maxHeight;
    [SerializeField] private LayerMask playerLayer;
    private float checkTimer;
    private bool attacking;
    private Vector3 destination;
    //specify an array for directions which will store exactly 4 elements, right/left/up/down movement
    private Vector3[] directions = new Vector3[4];

    [Header("Audio")]
    [SerializeField] private AudioClip impactSound;

    //setting it to not do anything on startup
    private void onEnable()
    {
        Stop();
    }

    // creating an enemy that will automatically home in on the player to attack 
    // storing player location in destination
    private void Update()
    {
        // moving the spikehead if it is attacking
        if (attacking)
        {
            transform.Translate(destination * Time.deltaTime * speed);
            MaxHeight();
        }
        else
        {
            // when not attacking we have the spikehead check for the player by incrementing
            // the check timer and once it is greater than the delay it will go to the player if near
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
                CheckForPlayer();
        }
    }

    //code to check for the player using a raycast in each of the four directions
    private void CheckForPlayer()
    {
        CalculateDirections();

        //iteration to check all four directions for the player
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i],range,playerLayer);

            // if we find the player switch to attack mode and seek out the player using the directions
            if (hit.collider != null && !attacking)
            {
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
            }
        }
    }

    private void CalculateDirections()
    {
        //calculating movement in the four directions right/left/up/down
        directions[0] = transform.right * range;
        directions[1] = -transform.right * range;
        directions[2] = transform.up * range;
        directions[3] = -transform.up * range;
    }

    //stopping the spikehead
    private void Stop()
    {
        // setting the position to be the current position
        destination = transform.position;
        attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Ground" )
        {
            SoundManager.instance.PlaySound(impactSound);
            base.OnTriggerEnter2D(collision);
            // stop the spikehead once it hits something
            Stop();
        }
    }

    private void MaxHeight()
    {
        if (transform.position.y >= maxHeight)
        {
            Stop();
        }
    }

}
