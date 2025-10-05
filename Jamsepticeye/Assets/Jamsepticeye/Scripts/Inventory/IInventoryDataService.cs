using UnityEngine;

public interface IInventoryDataService
{
    public static ObservableList<ItemData> ItemsInInventory { get; }
    public static ObservableList<ItemData> UsedItems { get; }

    void CollectItem(ItemData itemData);
    void UseItem(ItemData itemData);

    bool HasItem(ItemData item);

    bool HasUsedItem(ItemData item);

    public bool HasCollectedItem(ItemData item)
        => HasItem(item) || HasUsedItem(item);
}
