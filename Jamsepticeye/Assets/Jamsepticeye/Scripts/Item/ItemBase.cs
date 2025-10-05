using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField] protected ItemData itemData;

    public abstract void Init(ItemData itemData);

    protected void OnValidate()
        => Init(itemData);    
}


