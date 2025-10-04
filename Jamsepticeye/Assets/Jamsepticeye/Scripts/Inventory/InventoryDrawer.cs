using Ink.Parsed;
using UnityEngine;
using System.Collections.Generic;
using VInspector.Libs;

public class InventoryDrawer : MonoBehaviour
{
    [SerializeField] Item Item_Prefab;
    [SerializeField] Transform iconHolder;

    readonly Dictionary<ItemData, Item> ItemDataToInstance = new();

    void OnEnable()
    {
        InventoryDataManager.ItemsInInventory.ItemAdded += HandleItemCollected;
        InventoryDataManager.ItemsInInventory.ItemRemoved += HandleItemRemoved;
    }

    void OnDisable()
    {
        InventoryDataManager.ItemsInInventory.ItemAdded += HandleItemCollected;
        InventoryDataManager.ItemsInInventory.ItemRemoved += HandleItemRemoved;
    }

    void HandleItemCollected(ItemData itemData)
    {
        var itemIconInstance = Instantiate(Item_Prefab, iconHolder);
        ItemDataToInstance.Add(itemData, itemIconInstance);
    }

    void HandleItemRemoved(ItemData itemData)
    {
        var itemIconInstance = ItemDataToInstance[itemData];
        ItemDataToInstance.Remove(itemData);
        itemIconInstance.Destroy();
    }
}
