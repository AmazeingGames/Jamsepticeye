using UnityEngine;

public class GroceryStoreEggsScript : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        var dialogueInteraction = GetComponentInParent<DialogueInteraction>();
        if (dialogueInteraction != null)
        {
            if (GameStateScript.Instance.Is(GameState.HAS_EGGS))
            {
                dialogueInteraction.Disable();
                dialogueInteraction.HideIcon();
            } else if (GameStateScript.Instance.Is(GameState.NEEDS_EGGS))
            {
                dialogueInteraction.ShowIcon();
            }
            else
            {
                dialogueInteraction.Enable();
                dialogueInteraction.HideIcon();
            }
        }
    }
}
