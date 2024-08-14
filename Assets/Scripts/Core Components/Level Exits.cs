using UnityEngine;
using UnityEngine.SceneManagement;

public class levelExit : MonoBehaviour
{
    Scene scene;
    
    private void Update()
    {
        // making the current scene set to the scene variable
        scene = SceneManager.GetActiveScene();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(scene.buildIndex == 1)
            {
               //GameStateManager.Gyatt = true;
                SceneManager.LoadScene(2);
                GameStateManager.IsGameCompleted = true;
            }

            else if (scene.buildIndex == 3)
            {
                //GameStateManager.IsGameCompleted = true;
                SceneManager.LoadScene(0);
            }
        }
    }
}
