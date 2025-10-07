using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundTile : TileBase
{
    public enum Tile { Grass, Dirt, Stone, Wood }
    [field: SerializeField] public Tile MyTile { get; private set; }
    
}
