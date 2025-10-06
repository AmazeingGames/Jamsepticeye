using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneRootData", menuName = "Scriptable Objects/SceneRootData")]
public class SceneRootData : ScriptableObject
{
    [field: SerializeField] public FMODEvents.MusicType MyMusic { get; private set; }
    [field: SerializeField] public FMODEvents.AmbType MyAmbience { get; private set; }
    [field: SerializeField] public EventReference PlayerFootsteps { get; private set; }

}
