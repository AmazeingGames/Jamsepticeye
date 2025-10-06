using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private void OnEnable()
    {
        DialogueInteraction.DialogueInteractEventHandler += HandleDialogueInteraction;
        DialogueManager.StartDialogueLineEventHandler += HandleStartDialogueLine;
        UIButton.ButtonInteractEventHandler += 
    }

    private void OnDisable()
    {
        DialogueInteraction.DialogueInteractEventHandler -= HandleDialogueInteraction;
        DialogueManager.StartDialogueLineEventHandler -= HandleStartDialogueLine;
    }

    // Called when the player interacts with anything that triggers dialogue
    // This includes interacting with sticks, doors, npcs, etc.
    void HandleDialogueInteraction(object sender, DialogueInteraction.DialogueInteractEventArgs e)
    {

    }

    // Played every time there's a new line of dialogue
    void HandleStartDialogueLine(object sender, DialogueManager.StartDialogueLineEventArgs e)
    {
        switch (e.speaker.Name)
        {
            case "Baker":
                break;
            case "Grim Peeper":
                break;
            case "Tim":
                break;
            case "Young boy":
                break;
            case "DocDoor (Nurse)":
                break;

        }
    }

    // Called when interacting with ui buttons
    void HandleButtonInteract(object sender,  UIButton.ButtonInteractEventArgs e)
    {
        switch (e.myInteractType)
        {
            case UIButton.ButtonInteractEventArgs.InteractType.PointerEnter:
                break;
            case UIButton.ButtonInteractEventArgs.InteractType.PointerExit:
                break;
            case UIButton.ButtonInteractEventArgs.InteractType.PointerClick:
                break;
        }
    }


    void PlayAudio()
    {

    }
}
