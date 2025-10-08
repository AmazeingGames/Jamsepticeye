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
    [SerializeField] float teleportCooldownDuration = 5;
    float timeSinceLastTeleported;

    void Start()
    {
        timeSinceLastTeleported = teleportCooldownDuration;
    }

    private void Update()
    {
        timeSinceLastTeleported += Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (teleportCooldownDuration >= timeSinceLastTeleported)
            return;

        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            if (requiredGameState == new GameState() || GameStateScript.Instance.Is(requiredGameState))
            {
                timeSinceLastTeleported = teleportCooldownDuration;

                ServiceLocator.GetSceneHelperSerivce().EnableOnlyTargetScene(sceneDestination);
                SpawnPointHandler.TeleportToScene(spawnPoint);
            }
        }
    }
}
