using MoreMountains.Tools;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class ItemInteractable : ItemBase, IInteractable
{
    [SerializeField] protected GameObject interactIcon;

    [Header("Components")]
    [SerializeField] SpriteRenderer spriteRenderer;

    public GameObject InteractIcon => interactIcon;

    public override void Init(ItemData itemData)
    {
        this.itemData = itemData;
        spriteRenderer.sprite = itemData.InGameSprite;
    }

    public void Interact()
    {
        if (itemData.DisableSelfOnPickup)
            gameObject.SetActive(false);

        ServiceLocator.GetInventoryService().CollectItem(itemData);
    }

    // I'm assuming you'll always be able to collect an item you see
    public bool IsEnabled()
        => gameObject.activeInHierarchy;

    public bool CanInteract()
        => gameObject.activeInHierarchy && !ServiceLocator.GetInventoryService().HasItem(itemData);


    public void OnStart()
    {
        interactIcon.SetActive(false);
    }

    void Start()
    {
        OnStart();
    }
}
