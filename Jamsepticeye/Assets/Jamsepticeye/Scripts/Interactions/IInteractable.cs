using UnityEngine;

public interface IInteractable
{
    GameObject Icon { get; }

    void Interact();

    void SetIcon(bool active)
        => Icon.SetActive(active);
}
