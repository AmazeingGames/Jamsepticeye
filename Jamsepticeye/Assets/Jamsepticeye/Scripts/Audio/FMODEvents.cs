using FMOD.Studio;
using FMODUnity;
using MoreMountains.Tools;
using UnityEngine;

public class FMODEvents : MMSingleton<FMODEvents>
{
    // Improved code readability unfortunately creates tight coupling between this and the FMOD project
    // Enum names and int values should match 1 : 1 with the FMOD project
    public enum AmbType { None = -1, Village = 0, Store = 1 }
    public enum MusicType { None = -1, Menu = 0, OpeningSequence = 2, BakerMagic = 1, Village = 3, Grocery = 4, Bakery = 5, Ending = 6 }
    public enum FootstepType { None = -1, Grass = 0, Stone = 1 }

    [field: SerializeField] public EventReference Ambience_REF { get; private set; }
    [field: SerializeField] public EventReference FootSteps_REF { get; private set; }
    [field: SerializeField] public EventReference Music_REF { get; private set; }

    [Header("User Interface")]
    [field: SerializeField] public EventReference UIButtonClick { get; private set; }
    [field: SerializeField] public EventReference UIButtonHover { get; private set; }

    public EventInstance Ambience_INST { get; private set; }
    public EventInstance Music_INST { get; set; }

    void Start()
    {
        Ambience_INST = CreateInstance(Ambience_REF);
    }

    EventInstance CreateInstance(EventReference sound)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        return eventInstance;
    }
}
