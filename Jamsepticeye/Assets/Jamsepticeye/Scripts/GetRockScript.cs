using UnityEngine;

public class GetRockScript : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        var dialogueInteraction = GetComponentInParent<DialogueInteraction>();
        if (dialogueInteraction != null)
        {
            if (GameStateScript.instance.Is(GameState.NEEDS_ROCKS))
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
