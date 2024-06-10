using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    private Vector3[] initialPosition;

    private void Awake()
    {
        // saving positions of all the enemies
        // to make sure we have enough space to store it we set the initialPosition equal to the
        // length of a vector array with the length of enemies
        initialPosition = new Vector3[enemies.Length];
        // iterating through the enemies to store their positions
        for (int i = 0; i < enemies.Length; i++) 
        {
            // making sure that in the unity editor if we accidentally delete or mistype the array value
            // that we wont count the null values from it
            if (enemies[i] != null)
                initialPosition[i] = enemies[i].transform.position;
        }
    }

    public void ActivateRoom(bool _status)
    {
        //activating or deactivating the enemies in the room
        for (int i = 0; i < enemies.Length; i++)
        {
            // making sure that in the unity editor if we accidentally delete or mistype the array value
            // that we wont count the null values from it again
            if (enemies[i] != null)
            { 
                //setting their status to active or inactive and moving them back to original positions
                // if inactive
                enemies[i].SetActive(_status);
                enemies[i].transform.position = initialPosition[i];
               
            }
        }
    }
}
