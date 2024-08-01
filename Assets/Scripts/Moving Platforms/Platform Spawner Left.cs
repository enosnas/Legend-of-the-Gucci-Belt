using UnityEngine;

public class PlatformSpawnerLeft : MonoBehaviour
{
    [SerializeField] private float platformSendCooldown;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] platforms;
    private float cooldownTimer;

    private void Awake()
    {
        InitializePlatforms(); // Ensure platforms are properly initialized
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= platformSendCooldown)
            SendPlatform();
    }

    private void InitializePlatforms()
    {
        foreach (var platform in platforms)
        {
            platform.GetComponent<MovingPlatformLeft>().InitializePlatform();
        }
    }

    //sets the cooldown to 0 and will count up until it can attack again as set later in update
    // also resets the position of the platforms to the original position and then shoots in the
    // correct direction
    private void SendPlatform()
    {
        cooldownTimer = 0;
        platforms[FindPlatform()].transform.position = spawnPoint.position;
        platforms[FindPlatform()].GetComponent<MovingPlatformLeft>().ActivatePlatform();
    }

    // allows us to pool platforms instead of creating infinite, we will have a set that are usable
    private int FindPlatform()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            if (!platforms[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
