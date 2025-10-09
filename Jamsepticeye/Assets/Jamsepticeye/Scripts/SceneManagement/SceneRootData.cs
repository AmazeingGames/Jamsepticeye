using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneRootData", menuName = "ScriptableObjects/SceneRootData")]
public class SceneRootData : ScriptableObject
{
    public enum SceneType { None, Village, Bakery, GroceryStore, Menu, Bootstrap, Credits }
    [field: SerializeField] public SceneType MyScene { get; private set; } = SceneType.None;
}
