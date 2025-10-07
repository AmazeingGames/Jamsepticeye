using UnityEngine;
using UnityEngine.Tilemaps;

public interface ITilemapHelperService 
{
    DataTile GetTileUnderObject(GameObject gameObject);
    DataTile GetTileAtPosition(Vector3Int position);
}
