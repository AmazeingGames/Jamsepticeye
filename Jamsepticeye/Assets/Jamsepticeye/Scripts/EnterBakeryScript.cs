using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterBakeryScript : MonoBehaviour
{
    public void Update()
    {
        var dialogueInteraction = GetComponentInParent<DialogueInteraction>();
        var doorTeleport = GetComponentInParent<DoorTeleport>();
        if (dialogueInteraction != null)
        {
            if (GameStateScript.Instance.Is(GameState.ALLOWED_BAKERY) && !GameStateScript.Instance.Is(GameState.BAKER_DEAD))
            {
                dialogueInteraction.Disable();
                if (doorTeleport != null)
                {
                    doorTeleport.Enable();
                }
            }
            else
            {
                dialogueInteraction.Enable();
                if (doorTeleport != null)
                {
                    doorTeleport.Disable();
                }
            }
        }
    }
}

