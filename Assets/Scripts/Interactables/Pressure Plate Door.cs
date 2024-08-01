using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateDoor : MonoBehaviour
{
    private Animator anim;
    private PressurePlate plate;
    [SerializeField] GameObject parent;
    [SerializeField] private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        parent = transform.parent.gameObject;
        plate = parent.GetComponentInChildren<PressurePlate>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if(plate.doorOpen == true)
        {
            boxCollider.enabled = false;
            anim.SetBool("open", true);
        }
        else
        {
            boxCollider.enabled = true;
            anim.SetBool("open", false);
        }
    }
}
