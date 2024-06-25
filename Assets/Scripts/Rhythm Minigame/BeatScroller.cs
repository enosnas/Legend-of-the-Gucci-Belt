using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BeatScroller : MonoBehaviour
{
    public float beatTempo;
    public int combo = 0;
    private int dancePoints = 0;
    public int totalScore = 0;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;

    private void Start()
    {
        beatTempo = beatTempo / 60f;
    }

    private void Update()
    {
        if (!GameStateManager.minigameStart)
        {
            if (Input.anyKeyDown)
                GameStateManager.minigameStart = true;
        }

        if(GameStateManager.minigameEnd == true)
        {

        }
    }

    // adding score and combo text to ui
    public void NoteHit()
    {
        scoreText.text = "Score: " + totalScore;
        comboText.text = combo + " combo!";
    }

    //reseting combo text to nothing when a miss occurs
    public void NoteMiss()
    {
        Debug.Log("Note Missed");
        comboText.text = " ";
    }

    // method to add score and increment the score and combo ui elements
    public void AddScore(int points)
    {
        if (points > 333)
        {
            combo++;
            comboText.text = combo + " combo!";
        }
        else
        {
            combo = 0;
            comboText.text = " ";
        }

        totalScore += (points + (333 *combo));

        scoreText.text = "Score: " + totalScore;
    }

    // method to give player a grade based on ddr4th mix calculation
    public void AddGrade(int grade)
    {
        dancePoints += grade;
    }

    //A "Perfect" is worth 2 points A "Great" is worth 1 points A "Good" is worth 0 points A "Boo" will subtract 4 points A "Miss" will subtract 5 points
    //AA = All Perfect A = All Perfect and Great B = At least 64% of maximum Dance Points C = Less than 64% of maximum Dance Points D = Failed 
}
