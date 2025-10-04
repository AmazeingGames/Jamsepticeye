using UnityEngine;

public interface IInteractable
{
    GameObject Icon { get; }
    bool Enabled { get; }

    void Interact();
    void SetIcon(bool active)
    {
        if (Enabled)
            Icon.SetActive(active);
    }
}
