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
        if (!CanInteract())
            return;

        if (itemData.DisableSelfOnPickup)
            gameObject.SetActive(false);

        ServiceLocator.GetInventoryService().CollectItem(itemData.MyItemType);
    }

    // I'm assuming you'll always be able to collect an item you see
    public bool IsEnabled()
        => CanInteract();

    // Only collect items you've never collected before
    public bool CanInteract()
        => gameObject.activeInHierarchy && !ServiceLocator.GetInventoryService().HasCollectedItem(itemData);

    public void SetIcon(bool active)
    {
        //Assert.IsNotNull(InteractIcon, "Icon should not be null");

        if (!active)
        {
            InteractIcon.SetActive(active);
            return;
        }
        
        InteractIcon.SetActive(CanInteract());
    }

    public void OnStart()
        => interactIcon.SetActive(false);

    void Start()
        => OnStart();
}
