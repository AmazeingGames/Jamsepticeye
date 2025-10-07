using UnityEngine;
using UnityEngine.Tilemaps;
using VInspector.Libs;

public class TilemapHelper : MonoBehaviour, ITilemapHelperService
{
    [SerializeField] Tilemap groundTilemap;

    void Awake()
    {
        ServiceLocator.ProvideTilemapHelperService(this);
    }

    void GetTileUnderGameObject(GameObject gameObject)
        => GetTileAtPosition(Vector3Int.FloorToInt(gameObject.transform.position));

    void GetTileAtPosition(Vector3Int position)
    {
        TileBase tile = groundTilemap.GetTile(position);
    }
}
