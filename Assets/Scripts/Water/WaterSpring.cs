using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class WaterSpring : MonoBehaviour
{
    [Header("Water Object Assignments")]
    public Transform springTransform;
    [SerializeField] private SpriteShapeController spriteShapeController = null;

    [Header("Wave Point Parameters")]
    public float velocity = 0;
    public float force = 0;
    // current height
    public float height = 0f;
    // normal height
    private float target_height = 0f;
    // water resistance to player movement
    [SerializeField] float resistance = 40f;
    private int waveIndex = 0;
    private List<WaterSpring> springs = new();

    // setting the index, velocity, height, and target height of the water points
    public void Init(SpriteShapeController ssc)
    {

        var index = transform.GetSiblingIndex();
        waveIndex = index + 1;

        spriteShapeController = ssc;
        velocity = 0;
        height = transform.localPosition.y;
        target_height = transform.localPosition.y;
    }

    // with dampening
    // adding the dampening to the force
    public void WaveSpringUpdate(float springStiffness, float dampening)
    {
        height = transform.localPosition.y;
        // maximum extension
        var x = height - target_height;
        var loss = -dampening * velocity;

        force = -springStiffness * x + loss;
        velocity += force;
        var y = transform.localPosition.y;
        transform.localPosition = new Vector3(transform.localPosition.x, y + velocity, transform.localPosition.z);

    }
    public void WavePointUpdate()
    {
        if (spriteShapeController != null)
        {
            Spline waterSpline = spriteShapeController.spline;
            Vector3 wavePosition = waterSpline.GetPosition(waveIndex);
            waterSpline.SetPosition(waveIndex, new Vector3(wavePosition.x, transform.localPosition.y, wavePosition.z));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            var speed = rb.velocity;

            velocity += speed.y / resistance;

            // Call the Splash method on collision
            WaterShapeController waterShapeController = spriteShapeController.GetComponent<WaterShapeController>();
            if (waterShapeController != null)
            {
                int index = transform.GetSiblingIndex();
                waterShapeController.Splash(index, velocity);
            }
        }
    }
}