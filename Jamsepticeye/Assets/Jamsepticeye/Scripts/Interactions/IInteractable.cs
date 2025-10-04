using UnityEngine;

public interface IInteractable
{
    GameObject InteractIcon { get; }

    void Interact();

    bool IsEnabled() => true;

    void SetIcon(bool active)
        => InteractIcon.SetActive(active);
}
