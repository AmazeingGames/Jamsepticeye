using Ink.Parsed;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
using VInspector;

public class InventoryDrawer : MonoBehaviour
{
    [SerializeField] ItemUIIcon ItemUI_Prefab;
    [SerializeField] Transform iconHolder;

    public SerializedDictionary<ItemData, ItemUIIcon> ItemDataToIconInstance = new();

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
        Assert.IsNotNull(iconHolder, "UI icon holder has not been set.");
        var itemIconInstance = Instantiate(ItemUI_Prefab, iconHolder);
        itemIconInstance.Init(itemData);

        ItemDataToIconInstance.Add(itemData, itemIconInstance);
    }

    void HandleItemRemoved(ItemData itemData)
    {
        var itemIconInstance = ItemDataToIconInstance[itemData];
        ItemDataToIconInstance.Remove(itemData);
        itemIconInstance.gameObject.SetActive(false);
        Destroy(itemIconInstance);
    }
}
