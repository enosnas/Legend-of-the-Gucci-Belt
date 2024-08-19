using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSManager : MonoBehaviour
{
    [SerializeField] public int targetFPS;

    private void Start()
    {
        Application.targetFrameRate = targetFPS;
    }
}
