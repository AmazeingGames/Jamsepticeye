using UnityEngine;
using UnityEngine.Tilemaps;
using VInspector.Libs;

public class TilemapHelper : MonoBehaviour, ITilemapHelperService
{
    Tilemap groundOverlayTilemap;
    Tilemap groundTilemap;

    void Awake()
    {
        ServiceLocator.ProvideTilemapHelperService(this);
    }

    void OnEnable()
    {
        SceneRoot.EnablingRootEventHandler += Scenes_EnablingRoot;
    }

    private void OnDisable()
    {
        SceneRoot.EnablingRootEventHandler -= Scenes_EnablingRoot;
    }

    void Scenes_EnablingRoot(object sender, SceneRoot.EnablingRootEventArgs e)
    {
        if (e.isSettingActive)
        {
            groundOverlayTilemap = e.groundOverlayTilemap;
            groundTilemap = e.groundTilemap;
        }
    }

    public TileBase GetTileUnderObject(GameObject gameObject)
        => GetTileAtPosition(Vector3Int.FloorToInt(gameObject.transform.position));

    public TileBase GetTileAtPosition(Vector3Int position)
    {
        position.z = 0;
        TileBase overlayTile = groundOverlayTilemap.GetTile(position);
        Debug.Log($"Is overlay tile null? {overlayTile == null}");

        if (overlayTile != null)
            return overlayTile;

        var groundTile = groundTilemap.GetTile(position);
        Debug.Log($"Is ground tile null? {groundTile == null}");
        return groundTile;
    }
}
