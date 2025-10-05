using UnityEngine;

public class GetSticksScript : MonoBehaviour
{
    void Update()
    {
        var dialogueInteraction = GetComponentInParent<DialogueInteraction>();
        if (dialogueInteraction != null)
        {
            if (GameStateScript.instance.Is(GameState.NEEDS_STICKS))
            {
                dialogueInteraction.Enable();
            }
            else
            {
                dialogueInteraction.Disable();
            }
        }
    }
}
