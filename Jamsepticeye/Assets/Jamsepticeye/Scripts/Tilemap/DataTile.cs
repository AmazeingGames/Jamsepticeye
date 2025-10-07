using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DataTile : Tile
{
    public enum Category { None = 0, Grass = 1, Dirt = 2, Stone = 3}
    [Header("Custom Data")]
    [field: SerializeField] public Category MyTile { get; private set; }
    
}
