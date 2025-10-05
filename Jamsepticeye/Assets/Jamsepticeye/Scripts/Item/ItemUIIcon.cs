using UnityEngine;
using UnityEngine.UI;

public class ItemUIIcon : ItemBase
{
    [SerializeField] Image image;

    public override void Init(ItemData itemData)
    {
        this.itemData = itemData;
        image.sprite = itemData.UISprite;
    }
}
