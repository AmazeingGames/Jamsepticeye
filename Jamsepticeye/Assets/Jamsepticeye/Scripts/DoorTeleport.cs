using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTeleport : MonoBehaviour
{
    [SerializeField]
    private string sceneDestination;

    [SerializeField]
    private bool enabled_ = false;

    [SerializeField] float doorCooldownDuration = 5f;
    float timeSinceLastTeleport;

    public void Enable()
    {
        enabled_ = true;
    }

    public void Disable()
    {
        enabled_ = false;
    }

    private void Update()
    {
        timeSinceLastTeleport += Time.deltaTime;
    }

    public static EventHandler<TeleportingEventArgs> TeleportingEventHandler;

    public class TeleportingEventArgs : EventArgs
    {
        public TeleportingEventArgs()
        {

        }
    }

    void OnTeleporting()
    {

    }

    public void Teleport()
    {
        if (enabled_ && timeSinceLastTeleport >= 5)
        {
            ServiceLocator.GetSceneHelperSerivce().EnableOnlyTargetScene(sceneDestination);
            SpawnPointHandler.TeleportToScene(Vector2.zero);
        }
    }

}

