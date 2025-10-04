using Unity.VisualScripting;
using UnityEngine;

public class DialogueInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject interactIcon;
    [SerializeField] TextAsset inkJSON;
    GameObject IInteractable.Icon { get => interactIcon; }

    void IInteractable.Interact()
    {
        Debug.Log($"Trigger Interaction with {gameObject.name}");

        ServiceLocator.GetDialogueService().PlayDialogue(inkJSON);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactIcon.SetActive(false);
    }
}
