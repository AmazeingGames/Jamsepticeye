using EasyTextEffects;
using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// So much duplication with the dialogue manager indicates some refactoring should be done
public class CutscenesPlayer : MonoBehaviour, ICutscenesService
{
    public enum StateChange { None, Triggered, Exited, Continued }

    // Tried a new naming convention here, which I honestly dislike a lot
    [Header("Dialogue UI")]
    [SerializeField] Canvas cutscene_CANVAS;
    [SerializeField] TextMeshProUGUI dialogue_TMP;
    [SerializeField] TextEffect dialogue_EFFECT;
    [SerializeField] Image canContine_IMAGE;
    [SerializeField] Image textBox_IMAGE;
    [SerializeField] Image scene_IMAGE;

    [Header("Dialogue Effects")]
    [SerializeField] List<string> appearEffects;
    [SerializeField] List<string> disappearEffects;

    [SerializeField] CutsceneSequence startingCutscene;

    [Header("Scene Global Properties")]
    [SerializeField] float timeTillCanContinue = 1f;

    static bool hasPlayedOpening = false;

    bool CanContinueToNextLine
    {
        get => canContinueToNextLine;
        set
        {
            canContinueToNextLine = value;
            canContine_IMAGE.enabled = value;
        }
    }

    bool canContinueToNextLine;

    CutsceneSequence currentSequence;

    int sceneIndex;

    public static EventHandler<StateChangedEventArgs> StateChangedEventHandler;

    public class StateChangedEventArgs : EventArgs 
    { 
        public readonly StateChange myStateChange;
        public readonly CutsceneSequence cutsceneSequence;

        public StateChangedEventArgs(CutsceneSequence cutsceneSequence, StateChange myStateChange)
        {
            this.myStateChange = myStateChange;
            this.cutsceneSequence = cutsceneSequence;
        }
    }

    CutsceneScene CurrectScene => currentSequence.Scenes[sceneIndex];

    void Awake()
    {
        ServiceLocator.ProvideCutscenesService(this);
    }

    void Start()
    {
        ExitCutscene();
        if (!hasPlayedOpening)
        {
            hasPlayedOpening = true;
            // Resets state back to neutral
            TriggerCutsceneSequence(startingCutscene);
        }
    }

    [SerializeField] float spamPreventionTimer = .2f;

    bool isStoppingSpam;
    float timeSinceLastSpacePress;
    private void Update()
    {
        timeSinceLastSpacePress += Time.deltaTime;

        if (timeSinceLastSpacePress > spamPreventionTimer)
            isStoppingSpam = false;

        if (Input.GetKeyDown(KeyCode.Escape))
            ExitCutscene();

        if (isStoppingSpam)
            return;

        if (Input.GetButtonDown("Continue") && currentSequence != null && CanContinueToNextLine && !isStoppingSpam)
            DisplayNextScene();

        if (Input.GetButtonDown("Continue"))
        {
            isStoppingSpam = true;
            timeSinceLastSpacePress = 0;
        }
    }

    public void TriggerCutsceneSequence(CutsceneSequence cutsceneSequence)
    {
        OnTriggeringCutscene(cutsceneSequence);
        DisplayNextScene();
    }

    void OnTriggeringCutscene(CutsceneSequence cutsceneSequence)
    {
        cutscene_CANVAS.enabled = true;
        currentSequence = cutsceneSequence;
        sceneIndex = -1;

        OnStateChanged(StateChange.Triggered);
    }

    void DisplayNextScene()
    {
        sceneIndex++;
        CanContinueToNextLine = false;
        timeSinceLastSpacePress = 0;

        if (sceneIndex >= currentSequence.Scenes.Count)
        {
            ExitCutscene();
            return;
        }

        CutsceneScene scene = currentSequence.Scenes[sceneIndex];

        StartCoroutine(StartSceneEnd_CO());

        
        if (scene.HasText)
            StartDisplayingText(scene);

        if (scene.HasNewImage)
            scene_IMAGE.sprite = scene.SceneImage;

        if (scene.EntrySFX != "")
            Debug.Log($"Play sfx {scene.EntrySFX}");

        if (scene.EntryAnimation != null)
            Debug.Log($"Play entry animation {scene.EntryAnimation}");

        OnStateChanged(StateChange.Continued);
    }

    IEnumerator StartSceneEnd_CO()
    {
        yield return new WaitForSeconds(timeTillCanContinue);
        CanContinueToNextLine = true;
    }

    void SetTextVisibility(bool visible)
    {
        Debug.Log($"visible: {visible}");
        textBox_IMAGE.enabled = visible;
        dialogue_TMP.alpha = visible ? 255 : 0;

        if (!visible)
            dialogue_TMP.text = "";
        
    }


    bool isWaitingToPlayText;
    void StartDisplayingText(CutsceneScene scene)
    {
        // Finish hiding previous text before playing text
        // Resume execution on animation finish -> OnFinishDisappearAnimation()
        if (sceneIndex != 0 && !currentSequence.Scenes[sceneIndex - 1].HasText) // did previous scene have text?
        {
            foreach (string disappearEffect in disappearEffects)
                dialogue_EFFECT.StartManualEffect(disappearEffect);
            isWaitingToPlayText = true;
            return;
        }
        else
            PlayText(scene.Text);
    }

    void PlayText(string text)
    {
        dialogue_TMP.text = text;
        dialogue_TMP.color = CurrectScene.Color;

        foreach (string effect in appearEffects)
            dialogue_EFFECT.StartManualEffect(effect);

        SetTextVisibility(CurrectScene.HasText);
    }

    public void OnFinishTextDisappearAnimation()
    {
        if (isWaitingToPlayText)
            PlayText(currentSequence.Scenes[sceneIndex].Text);
    }

    void ExitCutscene()
    {
        OnExitedCutscene();

        if (currentSequence != null && currentSequence.DialogueToPlayOnEnd != null)
            ServiceLocator.GetDialogueService().PlayDialogue(currentSequence.DialogueToPlayOnEnd);
    }

    void OnExitedCutscene() 
    {
        sceneIndex = -1;
        currentSequence = null;
        cutscene_CANVAS.enabled = false;

        OnStateChanged(StateChange.Triggered);
    }

    void OnStateChanged(StateChange myStateChange)
    {
        StateChangedEventHandler?.Invoke(this, new StateChangedEventArgs(currentSequence, myStateChange));
    }
}
