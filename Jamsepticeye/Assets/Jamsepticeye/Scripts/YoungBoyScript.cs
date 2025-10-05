using UnityEngine;

public class YoungBoyScript : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        var dialogueInteraction = GetComponentInParent<DialogueInteraction>();
        if (dialogueInteraction != null)
        {
            if (GameStateScript.instance.Is(GameState.KID_FED))
            {
                dialogueInteraction.Disable();
            }
        }
    }
}
