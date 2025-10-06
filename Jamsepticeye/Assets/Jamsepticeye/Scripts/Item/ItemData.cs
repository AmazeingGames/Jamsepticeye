using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Rocks, Sugar, Eggs, Stick, Cookies, Coffee, Null }

    [field: SerializeField] public ItemType MyItemType { get; private set; }
    [field: SerializeField] public bool DisableSelfOnPickup { get; private set; }

    [field: SerializeField] public Sprite UISprite { get; private set; }
    [field: SerializeField] public Sprite InGameSprite { get; private set; }

}
