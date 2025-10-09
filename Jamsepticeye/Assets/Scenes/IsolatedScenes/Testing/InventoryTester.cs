using UnityEngine;
using System.Collections.Generic;

public class InventoryTester : MonoBehaviour
{
    [SerializeField] List<ItemData> itemsData;

    // Update is called once per frame
    void Update()
    {
#if DEBUG
        if (Input.GetKeyDown(KeyCode.Y))
            TestItem(itemsData[0]);

        if (Input.GetKeyDown(KeyCode.U))
            TestItem(itemsData[1]);

        if (Input.GetKeyDown(KeyCode.I))
            TestItem(itemsData[2]);

        if (Input.GetKeyDown(KeyCode.O))
            TestItem(itemsData[3]);

        if (Input.GetKeyDown(KeyCode.P))
            TestItem(itemsData[4]);
#endif
    }

    void TestItem(ItemData itemData)
    {
# if DEBUG
        if (InventoryDataManager.HasItem(itemData))
            ServiceLocator.GetInventoryService().UseItem(itemData);
        else
            ServiceLocator.GetInventoryService().CollectItem(itemData.MyItemType);
#endif
    }
}
