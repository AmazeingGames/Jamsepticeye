using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject interactIcon;
    [SerializeField] TextAsset inkJSON;

    bool enabled_ = true;
    bool iconEnabled_ = true;

    GameObject IInteractable.Icon { get => interactIcon; }

    bool IInteractable.IsEnabled()
    {
        return enabled_;
    }

    bool IInteractable.IsIconEnabled()
    {
        return iconEnabled_;
    }
    public void Interact()
    {
        if (enabled_)
        {
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (interactIcon != null)
            interactIcon.SetActive(false);
    }
}
