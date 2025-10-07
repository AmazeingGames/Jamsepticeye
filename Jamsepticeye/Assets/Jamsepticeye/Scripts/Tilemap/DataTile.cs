using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "DataTile", menuName = "ScriptableObjects/Tiles/DataTile")]
public class DataTile : Tile
{
    public enum Category { None = 0, Grass = 1, Dirt = 2, Stone = 3, Wood = 4, Tile = 5}
    [Header("Custom Data")]
    [field: SerializeField] public Category MyType { get; private set; }
    
}
