using UnityEngine;

public interface IInteractable
{
    GameObject Icon { get; }

    public void Interact();

    bool IsEnabled();
    bool IsIconEnabled();

    void SetIcon(bool active)
    {
        if (IsEnabled() && Icon != null && IsIconEnabled())
            Icon.SetActive(active);
    }
}
