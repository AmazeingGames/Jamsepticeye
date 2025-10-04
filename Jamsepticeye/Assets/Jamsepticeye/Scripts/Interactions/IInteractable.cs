using UnityEngine;

public interface IInteractable
{
    GameObject Icon { get; }

    void Interact();

    bool IsEnabled() => true;

    void SetIcon(bool active)
    {
        if (Icon != null)
            Icon.SetActive(active);
    }
}
