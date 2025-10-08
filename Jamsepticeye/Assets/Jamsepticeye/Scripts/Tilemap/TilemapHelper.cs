using UnityEngine;
using UnityEngine.Tilemaps;

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
        Debug.Log($"handled enabling root {e.rootData.MySceneType}");
        if (e.isSettingActive)
        {
            Debug.Log($"fully handled enabling root {e.rootData.MySceneType}");
            groundOverlayTilemap = e.groundOverlayTilemap;
            groundTilemap = e.groundTilemap;
        }
    }

    public DataTile GetTileUnderObject(GameObject gameObject)
        => GetTileAtPosition(Vector3Int.FloorToInt(gameObject.transform.position));

    public DataTile GetTileAtPosition(Vector3Int position)
    {
        Debug.Log($"isgroundoverlay null ? {groundOverlayTilemap == null}");
        position.z = 0;
        DataTile overlayTile = groundOverlayTilemap.GetTile(position) as DataTile;
        Debug.Log($"Is overlay tile null? {overlayTile == null}");

        if (overlayTile != null)
            return overlayTile;

        DataTile groundTile = groundTilemap.GetTile(position) as DataTile;
        Debug.Log($"Is ground tile null? {groundTile == null}");
        return groundTile;
    }
}
