using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneRootData", menuName = "ScriptableObjects/SceneRootData")]
public class SceneRootData : ScriptableObject
{
    [field: SerializeField] public FMODEvents.MusicType MyMusic { get; private set; } = FMODEvents.MusicType.None;
    [field: SerializeField] public FMODEvents.AmbType MyAmbience { get; private set; } = FMODEvents.AmbType.None;
}
