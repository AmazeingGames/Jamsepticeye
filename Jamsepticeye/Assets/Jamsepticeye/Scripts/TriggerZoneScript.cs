using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerZoneScript : MonoBehaviour
{
    [SerializeField]
    private string sceneDestination;

    [SerializeField]
    private Vector2 spawnPoint;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            SceneManager.LoadScene(sceneDestination);
            SpawnPointHandler.TeleportToScene(spawnPoint);
        }
    }
}
