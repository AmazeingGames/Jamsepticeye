using UnityEngine;

public class YoungBoyScript : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        var animator = GetComponentInParent<Animator>();
        if (animator != null)
        {
            bool isKidTalking = DialogueManager.GetInstance().IsDialoguePlaying && DialogueManager.GetInstance().currentSpeaker.Name == DialogueManager.GetInstance().kid.Name;
            animator.SetBool("IsTalking", isKidTalking);
        }
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
