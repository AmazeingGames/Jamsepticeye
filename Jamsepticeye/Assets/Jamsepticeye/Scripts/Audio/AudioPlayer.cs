using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using System;
using UnityEngine;
using static SceneRoot;
using static FMODEvents;
using System.Collections.Generic;

public class AudioPlayer : MonoBehaviour
{
    static bool hasStartedMusic = false;
    static AudioPlayer audioPlayerInstance;
    FMODEvents Events => FMODEvents.Instance;

    private Dictionary<Type, object> currentParameters = new();

    MusicType myMusicTypeBeforeCutscene = MusicType.None;

    [Header("Debug")]
    [SerializeField] bool startWithMusic_DEBUG;
    [SerializeField] bool startWithAmbience_DEBUG;

    EventInstance music_INST;
    EventInstance ambience_INSTambience_INST;

    bool hasEnabledVillageBefore;

    public MusicType MyCurrentMusic
    {
        get => currentParameters.TryGetValue(typeof(MusicType), out var value) ? (MusicType)value : MusicType.None;
        set => currentParameters[typeof(MusicType)] = value;
    }

    public AmbType MyCurrentAmbience
    {
        get => currentParameters.TryGetValue(typeof(AmbType), out var value) ? (AmbType)value : AmbType.None;
        set => currentParameters[typeof(AmbType)] = value;
    }

    public FootstepType MyCurrentFootstep
    {
        get => currentParameters.TryGetValue(typeof(FootstepType), out var value) ? (FootstepType)value : FootstepType.None;
        set => currentParameters[typeof(FootstepType)] = value;
    }

    private void OnEnable()
    {
        SceneRoot.EnablingRootEventHandler += Scenes_SetRootActive;
        CutscenesPlayer.StateChangedEventHandler += Cutscenes_StateChanged;
        Stepper.SteppedEventHandler += Entities_Stepped;
        UIButton.InteractingEventHandler += UI_Interacting;
        DialogueManager.StateChangedEventHandler += Dialogue_StateChanged;
        InventoryDataManager.ItemsInInventory.ItemAdded += Inventory_ItemAdded;
        InteractNestScript.BuildingHammockEventHandler += Cinematic_BuildingHammock;
        InteractNestScript.UpdatingCinematicEventHandler += Nest_UpdatingCinematic;
    }

    private void OnDisable()
    {
        SceneRoot.EnablingRootEventHandler -= Scenes_SetRootActive;
        CutscenesPlayer.StateChangedEventHandler -= Cutscenes_StateChanged;
        Stepper.SteppedEventHandler -= Entities_Stepped;
        UIButton.InteractingEventHandler -= UI_Interacting;
        DialogueManager.StateChangedEventHandler -= Dialogue_StateChanged;
        InventoryDataManager.ItemsInInventory.ItemAdded -= Inventory_ItemAdded;
        InteractNestScript.BuildingHammockEventHandler -= Cinematic_BuildingHammock;
        InteractNestScript.UpdatingCinematicEventHandler -= Nest_UpdatingCinematic;
    }

    private void Awake()
    {
        Debug.Log("More than one AudioPlayer should not be present in the scene.");

        audioPlayerInstance = this;
        currentParameters = new Dictionary<Type, object>
        {
            { typeof(AmbType), MyCurrentAmbience },
            { typeof(MusicType), MyCurrentMusic },
            { typeof(FootstepType), MyCurrentFootstep }
        };
    }

    private void Update()
    {
#if DEBUG
        if (Input.GetKeyDown(KeyCode.L))
        {
            Play(Events.NestFall_REF);
            Play(Events.ThrowRock_REF);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Play(Events.CashRegister_REF);
        }
#endif

    }

    private void Cinematic_BuildingHammock(object sender, InteractNestScript.BuildingHammockEventArgs e)
    {
        Play(Events.BuildHammock_REF);
    }

    private void Nest_UpdatingCinematic(object sender, InteractNestScript.UpdatingCinematicEventArgs e)
    {
        EventReference soundToPlay = e.myCinematicPoint switch
        {
            InteractNestScript.CinematicPoint.ThrowRock => Events.ThrowRock_REF,
            InteractNestScript.CinematicPoint.NestFall => Events.NestFall_REF,
            InteractNestScript.CinematicPoint.None => throw new NotImplementedException("Cinematic point not set."),
            InteractNestScript.CinematicPoint.Beginning or
            InteractNestScript.CinematicPoint.End => default,
            _ => throw new NotImplementedException("Switch expression is not exhaustive"),
        };
        Debug.Log($"{e.myCinematicPoint} => {soundToPlay}");
        Play(soundToPlay);
    }

    void Inventory_ItemAdded(ItemData itemAdded)
    {
        EventReference soundToPlay = itemAdded.MyItemType switch
        {
            ItemData.ItemType.Sugar => Events.CashRegister_REF,
            _ => default,
        };

        Play(soundToPlay);
    }


    void UI_Interacting(object sender, UIButton.InteractingEventArgs e)
    {
        Debug.Log("UI Interact handled");
        var soundToPlay = e.myInteraciton switch
        {
            UIButton.Interaction.Enter => Events.UIButtonHover,
            UIButton.Interaction.Click => Events.UIButtonClick,
            UIButton.Interaction.None => throw new NotImplementedException(),
            _ => default,
        };

        Play(soundToPlay);
    }

    void Dialogue_StateChanged(object sender, DialogueManager.StateChangedEventArgs e)
    {
        switch (e.myStateChange)
        {
            case DialogueManager.StateChange.None:
                break;
            case DialogueManager.StateChange.Triggered:
                break;
            case DialogueManager.StateChange.Exited:
                if (e.endGame)
                {
                    SetParameter(MusicType.Ending);
                }
                break;

            case DialogueManager.StateChange.Continued:
                FMODEvents.GarbleChar myGarbleParameter = e.speaker.MySpeaker switch
                {
                    Speaker.Character.Baker => GarbleChar.Baker,
                    Speaker.Character.Peeper => GarbleChar.Peeper,
                    Speaker.Character.Tim => GarbleChar.Tim,
                    Speaker.Character.HungryBoy => GarbleChar.Boy,
                    Speaker.Character.DocDoor => GarbleChar.Nurse,
                    Speaker.Character.None => throw new NotImplementedException("Speaker not set in DialogueManager struct"),
                    _ => throw new NotImplementedException("Switch expression is not exhaustive"),
                };
                SetParameter(myGarbleParameter);
                Play(Events.Dialogue);
                break;
        }
    }

    void Scenes_SetRootActive(object sender, EnablingRootEventArgs e)
    {
        Debug.Log($"Handled : {hasStartedMusic}");
        if (!hasStartedMusic)
        {
            hasStartedMusic = true;

            if (startWithMusic_DEBUG)
                Play(Events.Music_REF);

            if (startWithAmbience_DEBUG)
                Play(Events.Ambience_REF);
        }


        switch (e.rootData.MySceneType)
        {
            case SceneRootData.SceneType.None:
                throw new NotImplementedException("Scene type on RootData scriptable object not set.");

            case SceneRootData.SceneType.Village:
                if (hasEnabledVillageBefore)
                    Play(Events.DoorOpen_REF);

                hasEnabledVillageBefore = true;
                break;

            case SceneRootData.SceneType.Bakery:
                SetParameter(AmbType.Store);
                SetParameter(MusicType.Bakery);
                Play(Events.DoorOpen_REF);
                break;

            case SceneRootData.SceneType.GroceryStore:
                SetParameter(AmbType.Store);
                SetParameter(MusicType.Grocery);
                Play(Events.BellRing_REF);
                break;

            case SceneRootData.SceneType.Menu:
                SetParameter(MusicType.Menu);
                SetParameter(AmbType.Menu);
                break;

            case SceneRootData.SceneType.Credits:
                SetParameter(AmbType.Menu);
                break;

            case SceneRootData.SceneType.Bootstrap:
                break;

            default:
                throw new NotImplementedException("Scene type case not covered.");
        }
    }

    public void Play(EventReference sound)
        => RuntimeManager.PlayOneShot(sound);

    void Entities_Stepped(object sender, Stepper.SteppedEventArgs e)
    {
        FootstepType footstepType = e.dataTile.MyType switch
        {
            DataTile.Category.Grass => FootstepType.Grass,

            DataTile.Category.Dirt or
            DataTile.Category.Stone or
            DataTile.Category.Wood or
            DataTile.Category.Tile => FootstepType.Stone,

            DataTile.Category.None => throw new NotImplementedException($"DataTile scriptable object has not set {nameof(e.dataTile.MyType)}"),
            _ => throw new NotImplementedException("Switch expression is not exhaustive"),
        };
        SetParameter(footstepType);
        Play(Events.FootSteps_REF);
    }

    void Cutscenes_StateChanged(object sender, CutscenesPlayer.StateChangedEventArgs e)
    {
        switch (e.myStateChange)
        {
            case CutscenesPlayer.StateChange.Triggered:
                myMusicTypeBeforeCutscene = MyCurrentMusic;

                MusicType myMusicType = e.cutsceneSequence.MyCutscene switch
                {
                    CutsceneSequence.Cutscene.BakerMagic => MusicType.BakerMagic,
                    CutsceneSequence.Cutscene.OpeningSequence => MusicType.OpeningSequence,

                    CutsceneSequence.Cutscene.NotSet => throw new NotImplementedException("Cutscene type hasn't been set in the 'CutsceneSequence' ScriptableObject instance"),
                    _ => throw new NotImplementedException("Switch expression is not exhaustive"),
                };

                SetParameter(myMusicType);
                break;

            case CutscenesPlayer.StateChange.Exited:
                SetParameter(myMusicTypeBeforeCutscene);
                if (e.cutsceneSequence.MyCutscene == CutsceneSequence.Cutscene.OpeningSequence)
                    SetParameter(AmbType.Village);
                break;

            case CutscenesPlayer.StateChange.Continued:
                break;

            case CutscenesPlayer.StateChange.None:
                throw new NotImplementedException();
        }
    }
    void SetParameter<T>(T parameter) where T : Enum
    {
        if (currentParameters.TryGetValue(typeof(T), out var value))
            currentParameters[typeof(T)] = parameter;
        else
            Debug.LogWarning("No matching `CurrentParameter` variable found!");

        string typeName = typeof(T).Name;
        string parameterName = Enum.GetName(typeof(T), parameter);
        int parameterValue = Convert.ToInt32(parameter);

        string output = $"Set the parameter of \"{typeName}\" to \"{parameterValue} ({parameterName})\"";
        Debug.Log(output);

        RuntimeManager.StudioSystem.setParameterByName(typeName, parameterValue);
    }
}


