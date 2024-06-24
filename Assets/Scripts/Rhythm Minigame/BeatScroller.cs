using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    public float beatTempo;
    public static bool minigameStart { get; set; } = false;

    private void Start()
    {
        beatTempo = beatTempo / 60f;
    }

    private void Update()
    {
        if (!minigameStart)
        {
            if (Input.anyKeyDown)
                minigameStart = true;
        }
        else
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
    }
}
