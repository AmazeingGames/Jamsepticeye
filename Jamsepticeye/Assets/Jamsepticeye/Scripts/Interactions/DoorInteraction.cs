using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// For entering buildings
public class DoorInteraction : QuestInteraction
{
    [SerializeField]
    private string sceneDestination;

    new public void Start()
    {
        base.Start();
    }
    override protected void TriggerSuccess()
    {
        SceneManager.LoadScene(sceneDestination);
        SpawnPointHandler.TeleportToScene(Vector2.zero);
    }
}
