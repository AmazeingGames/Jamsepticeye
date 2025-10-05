using UnityEngine;

public interface IInteractable
{
    GameObject InteractIcon { get; }

    public void Interact();

    bool IsEnabled();
    bool IsIconEnabled();

    void SetIcon(bool active)
    {
        if (IsEnabled() && Icon != null && IsIconEnabled())
            Icon.SetActive(active);
    }

    /// <summary>
    ///     Disable interact icon on start.
    /// </summary>
    void OnStart();
}
