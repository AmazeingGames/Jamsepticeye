using UnityEngine;

public interface IInteractable
{
    GameObject InteractIcon { get; }

    public void Interact();

    bool IsEnabled();
    bool CanInteract();

    void SetIcon(bool active)
    {
        if (IsEnabled() && InteractIcon != null && CanInteract())
            InteractIcon.SetActive(active);
    }

    /// <summary>
    ///     Disable interact icon on start.
    /// </summary>
    void OnStart();
}
