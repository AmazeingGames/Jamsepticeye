using UnityEditor;
using UnityEngine;

public class BatchTileCreator
{
    [MenuItem("Assets/Create/Data Tiles From Sprites")]
    public static void CreateTilesFromSprites()
    {
        var sprites = Selection.GetFiltered<Sprite>(SelectionMode.Assets);

        foreach (var sprite in sprites)
        {
            DataTile tileData = ScriptableObject.CreateInstance<DataTile>();
            tileData.sprite = sprite;

            string spritePath = AssetDatabase.GetAssetPath(sprite);
            string spriteName = sprite.name;
            string directory = System.IO.Path.GetDirectoryName(spritePath);

            string assetPath = System.IO.Path.Combine(directory, spriteName + "_TileData.asset");
            AssetDatabase.CreateAsset(tileData, assetPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}