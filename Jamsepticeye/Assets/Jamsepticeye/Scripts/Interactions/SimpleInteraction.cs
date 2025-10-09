using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject interactIcon;

    [SerializeField] public bool enabled_ = true;

    public GameObject InteractIcon => interactIcon;

    bool IInteractable.IsEnabled()
        => enabled_;

    bool IInteractable.CanInteract()
        => !DialogueManager.IsDialoguePlaying;

    public void Interact()
    {
        if (enabled_)
        {
            Debug.Log($"Trigger Interaction with {gameObject.name}");
            GameStateScript.Instance.Set(GameState.HAS_COOKIES);
            ServiceLocator.GetInventoryService().CollectItem(ItemData.ItemType.Cookies);
            Disable();
        }
    }

    private void Disable()
    {
        enabled_ = false;
        if (interactIcon != null)
            interactIcon.SetActive(false);
        gameObject.SetActive(false);
    }
}
