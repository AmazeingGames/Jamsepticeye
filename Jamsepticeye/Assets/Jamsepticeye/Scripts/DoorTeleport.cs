using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTeleport : MonoBehaviour
{
    [SerializeField]
    private string sceneDestination;

    private bool enabled_ = false;

    public void Enable()
    {
        enabled_ = true;
    }

    public void Disable()
    {
        enabled_ = false;
    }
    public void Teleport()
    {
        if (enabled_)
        {
            SceneManager.LoadScene(sceneDestination);
            SpawnPointHandler.TeleportToScene(Vector2.zero);
        }
    }
}

