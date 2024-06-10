using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Creating a speed value to increase the player movement speed from the default -1 to 1
    // Also make it serialized which allows us to change it with an input field in the player object menu
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer; // creating a reference to the ground layer that our player will be standing on
    [SerializeField] private LayerMask wallLayer;    //creating wall layer
    private Rigidbody2D body; // Creating the reference to the rigidbody for player movement and naming it body
    private Animator anim;     // Creating the reference to the animator for player animation, not used but helpful
    private BoxCollider2D boxCollider;     // reference to our boxcollision
    private float gravityScale;     //storing gravity
    private float horizontalInput;     // store horizontal movement
    private PlayerAttack playerAttack;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private float volume;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;     // how much hang time allowed in the air before unable to jump
    private float coyoteCounter;     // how long since we run off an edge 

    [Header("Multi Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    private GameObject GunPoint;
    private GameObject Gun;

    // When the script is loaded/game is started the following will be called every time
    private void Awake()
    {
        // Get component will look through the attached object, in our case player, for Rigidbody2D
        // and assign it to body.
        body = GetComponent<Rigidbody2D>();
        // Do the same for the animator
        anim = GetComponent<Animator>();
        // Getting reference to the box collider on startup
        boxCollider = GetComponent <BoxCollider2D>();

        // setting game state to from game so we skip intro cutscene when going back to the main menu
        UIManager.FromGame = true;

        // re-enabling the gun for the player
        playerAttack = GetComponent<PlayerAttack>();
        GunPoint = transform.GetChild(0).gameObject;
        Gun = transform.GetChild(1).gameObject;

        // Check the condition and enable the PlayerAttack script if true
        if (GameStateManager.Gyatt == true)
        {
            playerAttack.enabled = true;
            Gun.SetActive(true);
            GunPoint.SetActive(true);
        }
    }

    // Every frame will be recorded and take input with update
    private void Update()
    {
        #region Horizontal Movement
        // Storing the horiztonal input in a float value for ease of use 
        float horizontalInput = Input.GetAxis("Horizontal");

        // flipping the player's direction when moving left or right
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(3,3,0);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-3,3,1);

        // Will apply velocity in directions based on a vector, our first is left and right movement 
        //which we obtain input using the Input.GetAxis property which will be defined when left/right 
        // or a/d is pressed and changes velocity on a scale from -1 to 1 in the x axis
        // we input nothing for the y movement as we do not want vertical movement
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        #endregion

        #region Jump Movement
        // Add jumping when the space bar is pressed
        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            Jump();
            // consider moving this to the animator as an event during the jump animation to make work
            // while holding down space bar
            //if(Input.GetKeyDown(KeyCode.Space) && isGrounded())
                //SoundManager.instance.PlaySound(jumpSound);
        }

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        // adjustable jump height code
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        // adding fast fall
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            FastFall();
        #endregion

        #region Grounded Check
        // coyote timer reset or decrease when in the air and multi jump reset on ground
        if (isGrounded())
        {
            coyoteCounter = coyoteTime;
            jumpCounter = extraJumps;
        }
        else
            coyoteCounter -= Time.deltaTime;

        // set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded",isGrounded());
        #endregion
    }

    #region Jump Logic
    private void Jump()
    {
        if (coyoteCounter <= 0 && jumpCounter <= 0) return;
        // RE-ENABLING THE HOLD JUMP but disabled the sound effect to prevent multi plays
        //SoundManager.instance.PlaySound(jumpSound);

        // Adding the jump code into here from the original update void
        if (isGrounded())
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        else
        {
            // allowing for coyote jump when not grounded
            if(coyoteCounter > 0)
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            // check for multi jump ability if ran out of coyote time
            else
            {
                if(jumpCounter > 0)
                {
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                    jumpCounter--;
                }
            }
        }

        // preventing double jumps off coyote jumps
        coyoteCounter = 0;
    }
    #endregion

    #region Fast Fall Logic
    private void FastFall()
    {
        if (!isGrounded())
            body.velocity = new Vector2(body.velocity.x, -jumpPower);
    }
    #endregion

    #region Collision Checks with Ground and Walls
    // Keeping track of whether the player is on the ground or not
    private bool isGrounded()
    {
        // this casts a virtual box in the direction 'direction' with length 'maxdistance' and checks whether there is a collision
        // if true then it returns true otherwise it returns false, we set the width to the player width for ease of checking for collision
        // first argument is the origin of the box, then the size of the box, followed by the angle of rotation, direction, position, and specific layer to check
        // we set these to the center of our player, the size of our player, angle to 0, direction to down, position is slightly below us, and check a ground layer

        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        // Check if the player is in contact with a wall
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    #endregion
}
