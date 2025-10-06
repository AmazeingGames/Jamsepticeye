using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject interactIcon;

    [SerializeField] bool enabled_ = true;

    public GameObject InteractIcon => interactIcon;

    bool IInteractable.IsEnabled()
        => enabled_;

    bool IInteractable.CanInteract()
        => !DialogueManager.GetInstance().IsDialoguePlaying;

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

    public void Disable()
    {
        enabled_ = false;
        if (interactIcon != null)
            interactIcon.SetActive(false);
    }



    void Start()
    {
    }
}
