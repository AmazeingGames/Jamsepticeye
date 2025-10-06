using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerZoneScript : MonoBehaviour
{
    [SerializeField]
    GameState requiredGameState = new GameState();
    [SerializeField]
    private string sceneDestination;

    [SerializeField]
    private Vector2 spawnPoint;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            if (requiredGameState == new GameState() || GameStateScript.Instance.Is(requiredGameState))
            {
                SceneManager.LoadScene(sceneDestination);
                SpawnPointHandler.TeleportToScene(spawnPoint);
            }
        }
    }
}
