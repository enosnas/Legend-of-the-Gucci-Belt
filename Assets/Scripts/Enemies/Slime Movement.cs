using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform pointR;
    [SerializeField] private Transform pointL;
    private Rigidbody2D body;
    private Animator anim;
    private FearLevel fearLevel;
    private Vector2 pointRInitialPos;
    private Vector2 pointLInitialPos;
    private Vector2 currentPointPos;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        fearLevel = GetComponent<FearLevel>();

        // Store initial positions
        pointRInitialPos = pointR.position;
        pointLInitialPos = pointL.position;
        currentPointPos = pointRInitialPos;

        anim.SetBool("running", true);
    }

    private void FixedUpdate()
    {
        MoveSlime();
        CheckDirection();
    }

    #region Slime Movement
    // the slime will start moving towards pointR and we will move it using vectors and then have it
    // change direction depending on which way it is moving
    private void MoveSlime()
    {
        if(fearLevel.Feared != true)
        {
            Vector2 direction = (currentPointPos - (Vector2)transform.position).normalized;
            body.velocity = new Vector2(direction.x * speed, body.velocity.y);

            if (currentPointPos == pointRInitialPos)
            {
                transform.localScale = new Vector3(-3, 3, 1);
            }
            else
            {
                transform.localScale = new Vector3(3, 3, 1);
            }
        }
        else
        {
            body.velocity = Vector2.zero;
        }
    }

    // once the slime comes within 0.5 x distance of the pointR or pointL it will switch direction
    // by default it will move to pointR first as set in the start method
    private void CheckDirection()
    {
        if (fearLevel.Feared != true)
        {
            if (Vector2.Distance(transform.position, currentPointPos) < 0.5f)
            {
                if (currentPointPos == pointRInitialPos)
                {
                    currentPointPos = pointLInitialPos;
                }
                else
                {
                    currentPointPos = pointRInitialPos;
                }
            }
        }
    }
    #endregion

    #region Disable on Death
    public void DeathDisable()
    {
        gameObject.SetActive(false);
    }
    #endregion

    // visualization of the pointR and pointL objects using gizmos
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointL.position, 0.5f);
        Gizmos.DrawWireSphere(pointR.position, 0.5f);
        Gizmos.DrawLine(pointL.position, pointR.position);
    }


}
