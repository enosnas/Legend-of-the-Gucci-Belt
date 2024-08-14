using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private InventoryManager inventory;

    private void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            {
            for(int i = 0; i < inventory.slots.Length; i++)
            {
                if(inventory.isFull[i] == false)
                {
                    inventory.isFull[i] = true;
                    break;
                }
            }
        }
    }
}
