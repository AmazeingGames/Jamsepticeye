using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using VInspector;

// I think Inventory Data Manager should be a service, located by a service locator. Scripts inform the inventory data manager directly regarding item changes, to which the data manager acts as a singular source of truth, which broadcasts its information to other scripts via events.
public class InventoryDataManager : MonoBehaviour, IInventoryDataService
{
    [SerializeField] SerializedDictionary<ItemData.ItemType, ItemData> itemTypeToData; 

    // Change to readonly lists in the future
    static readonly ObservableList<ItemData> itemsInInventory = new();
    static readonly ObservableList<ItemData> usedItems = new();
    public List<ItemData> itemsInInventoryProxy;
    public List<ItemData> usedItemsProxy;


    /// <summary>
    ///     DO NOT : ADD or REMOVE list elements outside of the owning class.
    /// </summary>
    public static ObservableList<ItemData> ItemsInInventory => itemsInInventory;

    /// <summary>
    ///     DO NOT : ADD or REMOVE list elements outside of the owning class.
    /// </summary>
    public static ObservableList<ItemData> UsedItems => usedItems;

    void Awake()
        => ServiceLocator.ProvideInventoryService(this);

    public void CollectItem(ItemData.ItemType itemType)
        => CollectItem(itemTypeToData[itemType]);
    public void CollectItem(ItemData itemData)
        => itemsInInventory.Add(itemData);

    public void UseItem(ItemData.ItemType itemType)
        => UseItem(itemTypeToData[itemType]);

    void Start()
    {
        StartCoroutine(UpdateProxyLists_CO());
    }

    void Update()
    {
        // Debug.Log("running");
        itemsInInventoryProxy.Clear();
        usedItemsProxy.Clear();

        if (!itemsInInventory.ContentsMatch(itemsInInventoryProxy))
        {
            usedItemsProxy.Clear();
            itemsInInventoryProxy.AddRange(itemsInInventory);
        }

        if (!usedItems.ContentsMatch(usedItemsProxy))
        {
            usedItemsProxy.Clear();
            usedItemsProxy.AddRange(usedItems);
        }
    }

    IEnumerator UpdateProxyLists_CO()
    {
        while (true)
        {
            

            yield return new WaitForSeconds(1);

           //  Debug.Log("running");
            foreach (var item in itemsInInventory)
                itemsInInventoryProxy.Add(item);

            

            itemsInInventoryProxy.AddRange(itemsInInventory);
            usedItemsProxy.AddRange(usedItems);
        }
    }

    public void UseItem(ItemData itemData)
    {
        if (!itemsInInventory.Contains(itemData))
        {
            Debug.Log("Attempting to use item that was never collected.");
            return;
        }

        // listeners are informed directly when items are removed from these classes via the observable list class
        itemsInInventory.Remove(itemData);
        usedItems.Add(itemData);
    }

    public bool HasItem(ItemData item)
        => itemsInInventory.Contains(item);

    public bool HasUsedItem(ItemData item)
        => usedItems.Contains(item);

    public bool HasCollectedItem(ItemData item)
        => HasItem(item) || HasUsedItem(item);
}
