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
        InventoryDataManager.ItemsInInventory.AddedItemEventHandler += HandleItemCollected;
        InventoryDataManager.ItemsInInventory.RemovedItemEventHandler += HandleItemRemoved;
    }

    void OnDisable()
    {
        InventoryDataManager.ItemsInInventory.AddedItemEventHandler -= HandleItemCollected;
        InventoryDataManager.ItemsInInventory.RemovedItemEventHandler -= HandleItemRemoved;
    }

    void HandleItemCollected(object sender, ItemData itemData)
    {
        Assert.IsNotNull(iconHolder, "UI icon holder has not been set.");
        var itemIconInstance = Instantiate(ItemUI_Prefab, iconHolder);
        itemIconInstance.Init(itemData);

        ItemDataToIconInstance.Add(itemData, itemIconInstance);
    }

    void HandleItemRemoved(object sender, ItemData itemData)
    {
        var icon = ItemDataToIconInstance[itemData];

        if (ItemDataToIconInstance[itemData] == null)
        {
            Debug.LogWarning("Item icon not in dictionary");
            return;
        }

        ItemDataToIconInstance.Remove(itemData);
        icon.gameObject.SetActive(false);
        Destroy(icon);
    }
}
