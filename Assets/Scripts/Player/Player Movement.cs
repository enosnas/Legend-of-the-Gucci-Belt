using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Creating a speed value to increase the player movement speed from the default -1 to 1
    // Also make it serialized which allows us to change it with an input field in the player object menu
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    private int jumpCounter = 1;

    [SerializeField] private LayerMask groundLayer; // creating a reference to the ground layer that our player will be standing on
    [SerializeField] private LayerMask waterLayer; // creating a reference to the water layer that our player will be swimming through
    [SerializeField] private LayerMask wallLayer;    //creating wall layer
    private float horizontalInput; // storing the horizontal input of the player
    private Rigidbody2D body; // Creating the reference to the rigidbody for player movement and naming it body
    private Animator anim;     // Creating the reference to the animator for player animation, not used but helpful
    private BoxCollider2D boxCollider;     // reference to our boxcollision
    private PlayerDig playerDig;
    private PlayerHowl playerHowl;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;     // how much hang time allowed in the air before unable to jump
    private float coyoteCounter;     // how long since we run off an edge 

    [Header("Wall Jump/Climb")]
    [SerializeField] private float wallslidingSpeed;
    private bool isWallsliding;
    private float walljumpingDirection;
    [SerializeField] private float walljumpingCooldown;
    [SerializeField] private float walljumpingDuration;
    [SerializeField] private float walljumpDistance;
    [SerializeField] private float walljumpHeight;

    [Header("Swim Jump Cooldown")]
    [SerializeField] private float swimJumpCooldown;
    private float swimJumpCooldownTimer;

    #region References
    // When the script is loaded/game is started the following will be called every time
    private void Awake()
    {
        // Get component will look through the attached object, in our case player, for Rigidbody2D
        // and assign it to body.
        body = GetComponent<Rigidbody2D>();
        // Do the same for the animator
        anim = GetComponent<Animator>();
        // Getting reference to the box collider on startup
        boxCollider = GetComponent<BoxCollider2D>();
        playerHowl = GetComponent<PlayerHowl>();

        // setting game state to from game so we skip intro cutscene when going back to the main menu
        UIManager.FromGame = true;

        // reference the dig for the player
        playerDig = GetComponent<PlayerDig>();

        // Check the condition and enable the playerDig script if true
        if (GameStateManager.Gyatt == true)
        {
            playerDig.enabled = true;
        }
    }
    #endregion

    // Every frame will be recorded and take input with update
    private void Update()
    {
        #region Horizontal Movement
        // Storing the horiztonal input in a float value for ease of use 
        horizontalInput = Input.GetAxis("Horizontal");

        // flipping the player's direction when moving left or right
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(3, 3, 1);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-3, 3, 1);

        // player movement
        // Will apply velocity in directions based on a vector, our first is left and right movement 
        //which we obtain input using the Input.GetAxis property which will be defined when left/right 
        // or a/d is pressed and changes velocity on a scale from -1 to 1 in the x axis
        // we input nothing for the y movement as we do not want vertical movement
        if(playerHowl.howling != true)
        {
            if (!onWall())
                body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            // slowing movement while swimming
            if (isSwimming())
                body.velocity = new Vector2(horizontalInput * speed / 2, body.velocity.y);
        }
        else
        {
            body.velocity = Vector2.zero;
        }
        #endregion

        #region Jump and Wall Jump Movement
        if (Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            if (GameStateManager.squirrelChosen == true)
                WallJump();
        }
        
        // adjustable jump height code
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        if(GameStateManager.squirrelChosen == true)
            WallSlide();

        #endregion

        #region Swim Movement
        // surfacing in water
        if (Input.GetKey(KeyCode.Space) && isSwimming())
            Surface();

        //adding diving
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && isSwimming())
            Diving();
        #endregion

        #region Grounded and Swimming and Climbing Check
        // coyote timer reset or decrease when in the air and multi jump reset on ground
        if (isGrounded())
        {
            coyoteCounter = coyoteTime;
            jumpCounter = 1;
        }
        else
            coyoteCounter -= Time.deltaTime;

        if (isSwimming())
            swimJumpCooldownTimer -= Time.deltaTime;
        else
            swimJumpCooldownTimer = swimJumpCooldown;

        if (onWall())
        {
            if (GameStateManager.squirrelChosen == true)
            {
                coyoteCounter = coyoteTime;
                jumpCounter = 1;
            }
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }
        #endregion

        #region Animation Parameters
        // set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());
        #endregion

        
    }

    #region Jump Logic
    private void Jump()
    {
        if (coyoteCounter <= 0 && jumpCounter <= 0) return;
        if (isSwimming() && swimJumpCooldownTimer > 0) return;
        if (isWallsliding == true) return;

        // Adding the jump code into here from the original update void
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        }
        else
        {
            // allowing for coyote jump when not grounded
            if (coyoteCounter > 0)
                body.velocity = new Vector2(body.velocity.x, jumpPower);
        }

        // preventing double jumps off coyote jumps
        coyoteCounter = 0;

        //removing 1 jump after jumping
        jumpCounter--;

        // setting cooldown timer to jump when player hits the water
        if (isSwimming())
        {
            swimJumpCooldownTimer = swimJumpCooldown;
        }
    }
    #endregion

    #region Wall Sliding and Jumping
    private void WallSlide()
    {
        if (onWall() && !isGrounded() && horizontalInput != 0)
        {
            isWallsliding = true;
            body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -wallslidingSpeed, float.MaxValue));
        }
        else
            isWallsliding = false;
    }

    private void WallJump()
    {
        if (coyoteCounter <= 0 && jumpCounter <= 0) return;
        if (isSwimming()) return;
        if (isGrounded()) return;

        if(isWallsliding)
            walljumpingDirection = -Mathf.Sign(transform.localScale.x); // turns player around when they start a wall jump
      
        if (onWall())
            body.velocity = new Vector2(body.velocity.x * walljumpingDirection / walljumpDistance, walljumpHeight);
        else
        {
            if (coyoteCounter > 0)
                body.velocity = new Vector2(body.velocity.x * walljumpingDirection / walljumpDistance, walljumpHeight);
        }

        // preventing double jumps off coyote jumps
        coyoteCounter = 0;

        //removing 1 jump after jumping
        jumpCounter--;

    }
    #endregion

    #region Swimming Logic
    private void Surface()
    {
        if (isSwimming() && swimJumpCooldownTimer > 0) return;

        if (isSwimming())
            body.velocity = new Vector2(body.velocity.x, jumpPower / 1.5f);
    }

    private void Diving()
    {
        if (isSwimming())
            body.velocity = new Vector2(body.velocity.x, -jumpPower / 2);
    }
    #endregion

    #region Collision Checks with Ground and Water and Wall
    // Keeping track of whether the player is on the ground or not
    public bool isGrounded()
    {
        // this casts a virtual box in the direction 'direction' with length 'maxdistance' and checks whether there is a collision
        // if true then it returns true otherwise it returns false, we set the width to the player width for ease of checking for collision
        // first argument is the origin of the box, then the size of the box, followed by the angle of rotation, direction, position, and specific layer to check
        // we set these to the center of our player, the size of our player, angle to 0, direction to down, position is slightly below us, and check a ground layer

        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    public bool isSwimming()
    {
        // this casts a virtual box in the direction 'direction' with length 'maxdistance' and checks whether there is a collision
        // if true then it returns true otherwise it returns false, we set the width to the player width for ease of checking for collision
        // first argument is the origin of the box, then the size of the box, followed by the angle of rotation, direction, position, and specific layer to check
        // we set these to the center of our player, the size of our player, angle to 0, direction to down, position is slightly below us, and check a ground layer

        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.up, 0.1f, waterLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    #endregion
}
