using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueZoneScript : MonoBehaviour
{
    [SerializeField]
    private GameObject expectedTrigger;

    [SerializeField]
    private DialogueInteraction dialogueInteraction;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == expectedTrigger)
        {
            if (dialogueInteraction != null)
            {
                dialogueInteraction.Interact();
            }
        }
    }
}
