using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DataTile : Tile
{
    public enum Category { Grass, Dirt, Stone, Wood }
    [field: SerializeField] public Category MyTile { get; private set; }
    
}
