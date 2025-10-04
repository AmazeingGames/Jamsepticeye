using System;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] ItemData itemData;
    [SerializeField] GameObject interactIcon;

    public static EventHandler<CollectItemEventArgs> CollectItemEventHandler;

    [Header("Components")]
    [SerializeField] Image Image;

    public GameObject InteractIcon => interactIcon;

    void Init()
    {
        Image.sprite = itemData.Icon;
    }

    void OnValidate()
    {
        Init();    
    }

    public void Interact()
        => OnCollectItem();

    void OnCollectItem()
    {
        gameObject.SetActive(false);
        CollectItemEventHandler?.Invoke(this, new(itemData));
    }    
}

public class CollectItemEventArgs : EventArgs
{
    public readonly ItemData itemData;
    public CollectItemEventArgs(ItemData itemData)
    {
        this.itemData = itemData;
    }
}
