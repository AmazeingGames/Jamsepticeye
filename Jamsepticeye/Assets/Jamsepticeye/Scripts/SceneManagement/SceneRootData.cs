using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneRootData", menuName = "ScriptableObjects/SceneRootData")]
public class SceneRootData : ScriptableObject
{
    // This should 100% be refactored to only have a RootScene type that AudioPlayer puts together to decide what sounds to play
    // This definitely doesn't feel like SOLID design to me, and I end up writing a lot of code for very little purpose
    [field: SerializeField] public FMODEvents.MusicType MyMusic { get; private set; } = FMODEvents.MusicType.None;
    [field: SerializeField] public FMODEvents.AmbType MyAmbience { get; private set; } = FMODEvents.AmbType.None;
    [field: SerializeField] public FMODEvents.StartSFX StartSFX { get; private set; } = FMODEvents.StartSFX.None;
}
