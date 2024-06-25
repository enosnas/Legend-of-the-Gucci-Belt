using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    [SerializeField] private KeyCode keytoPress;
    [SerializeField] private Transform rhythmButtonTransform;

    private bool canbePressed;
    private bool hasBeenHit = false;
    private BeatScroller beatScroller;

    private void Start()
    {
        beatScroller = GetComponentInParent<BeatScroller>();
    }

    // movement of the note, calculation of distance of note from corresponding button for score calculation, and setting note to hasBeenHit to avoid despawning and triggering miss due to exit
    private void Update()
    {
        if(GameStateManager.minigameStart == true)
            transform.position -= new Vector3(0f, beatScroller.beatTempo * Time.deltaTime, 0f);

        if (Input.GetKeyDown(keytoPress))
        {
            if (canbePressed)
            {
                float distance = Mathf.Abs(transform.position.y - rhythmButtonTransform.position.y);
                ScoreHit(distance);
                hasBeenHit = true;
                gameObject.SetActive(false);
            }
        }
    }

    // when note box collider contacts the button collider it can be hit
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Rhythm Button")
        {
            canbePressed = true;
        }
    }

    // when the note collider exits it counts a miss
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Rhythm Button")
        {
            canbePressed = false;
            if(!hasBeenHit)
                beatScroller.NoteMiss();
        }
    }

    // scoring mechanism based on the absolute distance from the button, based on ddr 4th mix scoring system
    private void ScoreHit(float distance)
    {
        int points = 0;
        int grade = 0;
        if (distance < 0.2f)
        {
            points = 777; // Perfect
            grade = 2;
            Debug.Log("Perfect Hit!");
        }
        else if (distance < 0.3f)
        {
            points = 555; // Great
            grade = 1;
            Debug.Log("Great Hit!");
        }
        else if (distance < 0.45f)
        {
            points = 333; // Good
            grade = 0;
            Debug.Log("Good Hit!");
        }
        else if (distance < 1.55f)
        {
            points = 111; // Bad
            grade = -4;
            Debug.Log("Bad Hit!");
        }

        beatScroller.AddScore(points);
        beatScroller.NoteHit();
        beatScroller.AddGrade(grade);
    }
}
