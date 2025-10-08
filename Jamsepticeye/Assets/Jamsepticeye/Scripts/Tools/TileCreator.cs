using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.IO;
using System.Linq;

public class TileCreator
{
/*    // [CreateAssetMenu()]
    public static void CreateTilesFromMultipleSprites()
    {
        // Get the selected Texture2D assets
        var textures = Selection.GetFiltered<Texture2D>(SelectionMode.Assets);

        foreach (var texture in textures)
        {
            // Load all sub-sprites from the texture (must be Sprite Mode Multiple)
            string assetPath = AssetDatabase.GetAssetPath(texture);
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(assetPath).OfType<Sprite>().ToArray();

            // For each sub-sprite create your ScriptableObject
            foreach (var sprite in sprites)
            {
                if (sprite == null) continue;

                // Create instance of your ScriptableObject
                DataTile tileData = ScriptableObject.CreateInstance<DataTile>();
                tileData.sprite = sprite;

                string directory = Path.GetDirectoryName(assetPath);
                string assetName = sprite.name + "_TileData.asset";
                string savePath = Path.Combine(directory, assetName);

                AssetDatabase.CreateAsset(tileData, savePath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Created tiles from multiple sprites.");
    }
#endif
*/
}
