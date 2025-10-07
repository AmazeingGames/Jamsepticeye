using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using System;
using UnityEngine;
using static SceneRoot;
using static FMODEvents;
using System.Collections.Generic;
using Unity.VisualScripting;
using Ink.Parsed;
using UnityEngine.Assertions;

public class AudioPlayer : MonoBehaviour
{
    static bool hasStartedMusic;
    static AudioPlayer audioPlayerInstance;
    FMODEvents Events => FMODEvents.Instance;

    private Dictionary<Type, object> currentParameters = new();

    MusicType myMusicTypeBeforeCutscene = MusicType.None;

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
        SceneRoot.SettingActiveEventHandler += Scenes_SetRootActive;
        CutscenesPlayer.StateChangedEventHandler += Cutscenes_StateChanged;
        Stepper.SteppedEventHandler += Entities_Stepped;
        UIButton.InteractingEventHandler += UI_Interacting;
        DialogueManager.StateChangedEventHandler += Dialogue_StateChanged;

    }
    
    private void OnDisable()
    {
        SceneRoot.SettingActiveEventHandler -= Scenes_SetRootActive;
        CutscenesPlayer.StateChangedEventHandler -= Cutscenes_StateChanged;
        Stepper.SteppedEventHandler -= Entities_Stepped;
        UIButton.InteractingEventHandler -= UI_Interacting;
        DialogueManager.StateChangedEventHandler -= Dialogue_StateChanged;
    }

    private void Awake()
    {
        Assert.IsNull(audioPlayerInstance, "More than one AudioPlayer should not be present in the sscene.");

        audioPlayerInstance = this;
        currentParameters = new Dictionary<Type, object>
        {
            { typeof(AmbType), MyCurrentAmbience },
            { typeof(MusicType), MyCurrentMusic },
            { typeof(FootstepType), MyCurrentFootstep }
        };
    }

    void UI_Interacting(object sender, UIButton.InteractingEventArgs e)
    {
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
        
    }

    void Scenes_SetRootActive(object sender, SetRootActiveEventArgs e)
    {
        if (!hasStartedMusic)
        {
            hasStartedMusic = true;

            Play(Events.Music_REF);
        }

        SetParameter(e.rootData.MyAmbience);
        SetParameter(e.rootData.MyMusic);
    }

    public void Play(EventReference sound)
        => RuntimeManager.PlayOneShot(sound);

    void Entities_Stepped(object sender, Stepper.SteppedEventArgs e)
    {
        FootstepType footstepType = e.dataTile.MyType switch
        {
            DataTile.Category.Grass => FootstepType.Grass,

            DataTile.Category.Dirt  or
            DataTile.Category.Stone or
            DataTile.Category.Wood  or
            DataTile.Category.Tile  => FootstepType.Stone,

            DataTile.Category.None  => throw new NotImplementedException("Category of 'TileData' scriptable object is not set"),
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

                    CutsceneSequence.Cutscene.NotSet => throw new NotImplementedException("Cutscene type hasn't been defined in the Scriptable Object"),
                    _ => throw new NotImplementedException("Switch expression is not exhaustive"),
                };

                SetParameter(myMusicType);
            break;

            case CutscenesPlayer.StateChange.Exited:
                SetParameter(myMusicTypeBeforeCutscene);
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

// Doesn't play music if music is already playing;
/*void PlayMusic(EventReference musicEvent)
{
    if (Events.Music_INST.isValid())
    {
        Events.Music_INST.getPlaybackState(out PLAYBACK_STATE state);

        if (state == FMOD.Studio.PLAYBACK_STATE.PLAYING)
            return;

        Events.Music_INST.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        Events.Music_INST.release();
    }

    Events.Music_INST = RuntimeManager.CreateInstance(musicEvent);
    Events.Music_INST.start();
}*/
