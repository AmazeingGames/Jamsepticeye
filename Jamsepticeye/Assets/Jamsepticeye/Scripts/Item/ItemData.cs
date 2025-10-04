using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Rocks, Sugar, Eggs, Stick, Cookies }

    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public ItemType MyItemType { get; private set; }

}
