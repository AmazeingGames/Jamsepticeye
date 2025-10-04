using UnityEngine;

public class DialogueInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject interactIcon;
    GameObject IInteractable.Icon { get => interactIcon; }

    void IInteractable.Interact()
    {
        Debug.Log("Trigger Interaction");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
