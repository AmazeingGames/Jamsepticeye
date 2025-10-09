using UnityEngine;

public interface IInventoryDataService
{
    public static ObservableList<ItemData> ItemsInInventory { get; }
    public static ObservableList<ItemData> UsedItems { get; }


    public void CollectItem(ItemData.ItemType itemType);

    public void UseItem(ItemData.ItemType itemType);

    void CollectItem(ItemData itemData);
    void UseItem(ItemData itemData);
}
