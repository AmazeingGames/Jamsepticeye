using UnityEngine;
using UnityEngine.Tilemaps;

public interface ITilemapHelperService 
{
    TileBase GetTileUnderObject(GameObject gameObject);
    TileBase GetTileAtPosition(Vector3Int position);
}
