using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Interact : MonoBehaviour
{
    [SerializeField] float interactRadius;

    IInteractable lastInteractable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        // Find the closest object with the interactable component
        Collider2D[] objectsInRange = new Collider2D[50];
        objectsInRange = Physics2D.OverlapCircleAll(transform.position, interactRadius);
        List<(GameObject, IInteractable)> interactbleObjects = new();

        foreach (Collider2D collider in objectsInRange)
        {
            if (collider.TryGetComponent<IInteractable>(out var interactable))
                interactbleObjects.Add((collider.gameObject, interactable));
        }

        (GameObject, IInteractable) closestInteractableObject = interactbleObjects
            .OrderBy(io => (io.Item1.transform.position - transform.position).sqrMagnitude)
            .FirstOrDefault();

        // Enable/Disable the Interact icon
        // Trigger interaction on keypress
        if (interactbleObjects.Count > 0)
        {
            if (lastInteractable != null && lastInteractable != closestInteractableObject.Item2)
                lastInteractable.SetIcon(false);

            lastInteractable = closestInteractableObject.Item2;
            closestInteractableObject.Item2.SetIcon(true);

            if (Input.GetButtonDown("Interact"))
                closestInteractableObject.Item2.Interact();
        }
        else if (lastInteractable != null)
        {
            lastInteractable.SetIcon(false);
            lastInteractable = null;
        }
    }
}
