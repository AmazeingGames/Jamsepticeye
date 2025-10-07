using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneRootData", menuName = "ScriptableObjects/SceneRootData")]
public class SceneRootData : ScriptableObject
{
    public enum SceneType { None, Village, Bakery, GroceryStore, Menu, Bootstrap }
    [field: SerializeField] public SceneType MySceneType { get; private set; } = SceneType.None;
}
