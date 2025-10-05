using Ink.Parsed;
using UnityEngine;
using System.Collections.Generic;
using VInspector.Libs;

public class InventoryDrawer : MonoBehaviour
{
    [SerializeField] ItemUIIcon ItemUI_Prefab;
    [SerializeField] Transform iconHolder;

    readonly Dictionary<ItemData, ItemUIIcon> ItemDataToInstance = new();

    void OnEnable()
    {
        InventoryDataManager.ItemsInInventory.ItemAdded += HandleItemCollected;
        InventoryDataManager.ItemsInInventory.ItemRemoved += HandleItemRemoved;
    }

    void OnDisable()
    {
        InventoryDataManager.ItemsInInventory.ItemAdded -= HandleItemCollected;
        InventoryDataManager.ItemsInInventory.ItemRemoved -= HandleItemRemoved;
    }

    void HandleItemCollected(ItemData itemData)
    {
        var itemIconInstance = Instantiate(ItemUI_Prefab, iconHolder);
        itemIconInstance.Init(itemData);

        ItemDataToInstance.Add(itemData, itemIconInstance);
    }

    void HandleItemRemoved(ItemData itemData)
    {
        var itemIconInstance = ItemDataToInstance[itemData];
        ItemDataToInstance.Remove(itemData);
        itemIconInstance.gameObject.SetActive(false);
        itemIconInstance.Destroy();
    }
}
