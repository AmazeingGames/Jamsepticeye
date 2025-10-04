using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPointHandler : MonoBehaviour
{
    public static Vector2 targetPosition;
    public static bool shouldTeleport = false;

    public static void TeleportToScene(Vector2 spawnPosition)
    {
        targetPosition = spawnPosition;
        shouldTeleport = true;
    }
}
