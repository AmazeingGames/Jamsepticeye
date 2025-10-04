using UnityEngine;

public class TriggerZoneScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.GetComponent<TeleportScript>().Teleport();
        }
    }
}
