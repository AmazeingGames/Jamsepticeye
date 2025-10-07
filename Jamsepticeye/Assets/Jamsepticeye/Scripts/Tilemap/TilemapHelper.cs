using UnityEngine;
using UnityEngine.Tilemaps;
using VInspector.Libs;

public class TilemapHelper : MonoBehaviour, ITilemapHelperService
{
    [SerializeField] Tilemap groundOverlayTilemap;
    [SerializeField] Tilemap groundTilemap;

    void Awake()
    {
        ServiceLocator.ProvideTilemapHelperService(this);
    }

    public TileBase GetTileUnderObject(GameObject gameObject)
        => GetTileAtPosition(Vector3Int.FloorToInt(gameObject.transform.position));

    public TileBase GetTileAtPosition(Vector3Int position)
    {
        TileBase overlayTile = groundOverlayTilemap.GetTile(position);

        if (overlayTile != null)
            return overlayTile;

        return groundTilemap.GetTile(position);
    }
}
