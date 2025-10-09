using FMOD.Studio;
using FMODUnity;
using MoreMountains.Tools;
using UnityEngine;

public class FMODEvents : MMSingleton<FMODEvents>
{
    // Enum names and int values should match 1 : 1 with the FMOD project
    public enum AmbType { None = -1, Village = 0, Store = 1, Menu = 2 }
    public enum MusicType { None = -1, Menu = 0, OpeningSequence = 2, BakerMagic = 1, Village = 3, Grocery = 4, Bakery = 5, Ending = 6 }
    public enum FootstepType { None = -1, Grass = 0, Stone = 1 }
    public enum GarbleChar { None = -1, Tim = 0, Nurse = 1, Baker = 2, Peeper = 3, Boy = 4}

    [field: SerializeField] public EventReference Ambience_REF { get; private set; }
    [field: SerializeField] public EventReference FootSteps_REF { get; private set; }
    [field: SerializeField] public EventReference Music_REF { get; private set; }
    [field: SerializeField] public EventReference Dialogue { get; private set; }

    [field: Header("Egg Sequence")]
    [field: SerializeField] public EventReference BuildHammock_REF { get; private set; }
    [field: SerializeField] public EventReference ThrowRock_REF { get; private set; }
    [field: SerializeField] public EventReference NestFall_REF { get; private set; }

    [field: Header("Store")]
    [field: SerializeField] public EventReference CashRegister_REF { get; private set; }

    [field: SerializeField] public EventReference DoorOpen_REF { get; private set; }
    [field: SerializeField] public EventReference BellRing_REF { get; private set; }

    [Header("User Interface")]
    [field: SerializeField] public EventReference UIButtonClick { get; private set; }
    [field: SerializeField] public EventReference UIButtonHover { get; private set; }


    static EventInstance CreateInstance(EventReference sound)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        return eventInstance;
    }
}
