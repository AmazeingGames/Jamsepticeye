using UnityEngine;

public interface IInventoryDataService
{
    public static ObservableList<ItemData> ItemsInInventory { get; }
    public static ObservableList<ItemData> UsedItems { get; }

    void CollectItem(ItemData itemData);
    void UseItem(ItemData itemData);

    bool IsItemInInventory(ItemData item);

    bool HasUsedItem(ItemData item);

    public bool HasCollectedItem(ItemData item)
        => IsItemInInventory(item) || HasUsedItem(item);
}
