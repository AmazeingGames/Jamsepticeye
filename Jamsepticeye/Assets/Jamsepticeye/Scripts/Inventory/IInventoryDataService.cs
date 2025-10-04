using UnityEngine;

public interface IInventoryDataService
{
    public static ObservableList<ItemData> ItemsInInventory { get; }
    public static ObservableList<ItemData> UsedItems { get; }

    void StoreItem(ItemData itemData);
    void UseItem(ItemData itemData);
}
