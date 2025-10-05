using UnityEngine;

public class CoffeeScript : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        var dialogueInteraction = GetComponentInParent<DialogueInteraction>();
        if (dialogueInteraction != null)
        {
            if (GameStateScript.instance.Is(GameState.HAS_COFFEE))
            {
                dialogueInteraction.Disable();
            }
            else
            {
                dialogueInteraction.Enable();
            }
        }
    }
}
