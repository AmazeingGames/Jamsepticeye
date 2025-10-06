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

    private Dictionary<Type, object> currentParameters;

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
        CutscenesPlayer.TriggeringCutsceneEventHandler += Cutscenes_TriggeringCutscene;
        CutscenesPlayer.ExitedCutsceneEventHandler += Cutscenes_ExitedCutscene;
    }

    
    private void OnDisable()
    {
        SceneRoot.SettingActiveEventHandler -= Scenes_SetRootActive;
        CutscenesPlayer.TriggeringCutsceneEventHandler -= Cutscenes_TriggeringCutscene;
        CutscenesPlayer.ExitedCutsceneEventHandler -= Cutscenes_ExitedCutscene;
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

    void Scenes_SetRootActive(object sender, SetRootActiveEventArgs e)
    {
        if (!hasStartedMusic)
        {
            hasStartedMusic = true;

            Play(Events.Music_REF);
        }

        // Set music and ambience
        // Start ambience onc on start and only adjust parameters

        SetParameter(e.rootData.MyAmbience);
        SetParameter(e.rootData.MyMusic);
    }

    public void Play(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
    }

    void Cutscenes_TriggeringCutscene(object sender, CutscenesPlayer.TriggeringCutsceneEventArgs e)
    {
        myMusicTypeBeforeCutscene = MyCurrentMusic;

        switch (e.cutsceneSequence.MyCutscene)
        {
            case CutsceneSequence.Cutscene.NotSet:
                throw new NotImplementedException("Cutscene type hasn't been defined in the Scriptable Object");

            case CutsceneSequence.Cutscene.BakerMagic:
                SetParameter(MusicType.BakerMagic);
                break;

            case CutsceneSequence.Cutscene.OpeningSequence:
                SetParameter(MusicType.OpeningSequence);
                break;
        }

        Play(Events.Music_REF);
    }

    void Cutscenes_ExitedCutscene(object sender, CutscenesPlayer.ExitedCutsceneEventArgs e)
    {
        SetParameter(MusicType.BakerMagic);
        Play(Events.Music_REF);
    }

    void Player_Stepped(object sender, FootstepType e)
    {
        switch (e)
        {
            case FootstepType.Grass:
                SetParameter(FootstepType.Grass);
                break;
                
            case FootstepType.Stone:
                SetParameter(FootstepType.Stone);
                break;
        }

        Play(Events.FootSteps_REF);
    }


    void SetParameter<T>(T parameter) where T : Enum
    {
        if (currentParameters.TryGetValue(typeof(T), out var value))
            currentParameters[typeof(T)] = parameter;
        else
            Debug.LogWarning("No matching `CurrentParameter` variable found!");

        string typeName = typeof(T).Name; // Gets the enum type name, e.g., "MusicType"
        string parameterName = Enum.GetName(typeof(T), parameter); // Gets the value name, e.g., "Ambience"
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
