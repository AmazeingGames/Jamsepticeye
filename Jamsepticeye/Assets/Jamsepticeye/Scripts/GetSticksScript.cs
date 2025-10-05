using UnityEngine;

public class GetSticksScript : MonoBehaviour
{
    void Start()
    {

    }

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
