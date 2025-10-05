using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject interactIcon;
    [SerializeField] TextAsset inkJSON;

    [SerializeField] bool enabled_ = true;
    bool iconEnabled_ = true;

    public GameObject InteractIcon => interactIcon;

    bool IInteractable.IsEnabled()
        => enabled_;

    bool IInteractable.CanInteract()
        => iconEnabled_ && !DialogueManager.GetInstance().IsDialoguePlaying;

    public void Interact()
    {
        if (enabled_)
        {
            if (DialogueManager.GetInstance().IsDialoguePlaying)
                return;

            Debug.Log($"Trigger Interaction with {gameObject.name}");
            ServiceLocator.GetDialogueService().PlayDialogue(inkJSON);
        }

        {
            var doorTeleport = GetComponentInParent<DoorTeleport>();
            if (doorTeleport != null)
            {
                doorTeleport.Teleport();
            }
        }
    }

    public void Disable()
    {
        enabled_ = false;
        if (interactIcon != null)
            interactIcon.SetActive(false);
    }

    public void Enable()
    {
        enabled_ = true;
    }


    public void HideIcon()
    {
        iconEnabled_ = false;
    }

    public void ShowIcon()
    {
        iconEnabled_ = true;
    }

    void Start()
    {
        if (interactIcon != null)
            interactIcon.SetActive(false);
    }

    public void OnStart()
    {
        throw new System.NotImplementedException();
    }
}
