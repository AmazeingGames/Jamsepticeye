using Unity.VisualScripting;
using UnityEngine;

// For entering buildings
public class DoorInteraction : QuestInteraction
{
    private TeleportScript teleportScript;

    new public void Start()
    {
        base.Start();
        teleportScript = GetComponent<TeleportScript>();
    }
    override protected void TriggerSuccess()
    {
        if (teleportScript != null)
        {
            teleportScript.Teleport();
        }
    }
}
