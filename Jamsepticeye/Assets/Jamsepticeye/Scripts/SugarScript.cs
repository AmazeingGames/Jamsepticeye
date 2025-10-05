using UnityEngine;

public class SugarScript : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        var dialogueInteraction = GetComponentInParent<DialogueInteraction>();
        if (dialogueInteraction != null)
        {
            if (GameStateScript.instance.Is(GameState.NEEDS_SUGAR))
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
