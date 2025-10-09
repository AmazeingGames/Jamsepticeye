using DG.Tweening;
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

// Should likely seprate into 2 parts: a 'CutscePlayer' and a 'CutsceneAnimator', one part handling the simulation and another part the animation
// So much duplication with the dialogue manager indicates some refactoring should be done
public class CutscenesPlayer : MonoBehaviour, ICutscenesService
{
    public enum StateChange { None, Triggered, Exited, Continued }

    // Tried a new naming convention here, which I honestly dislike a lot
    [Header("Dialogue UI")]
    [SerializeField] Canvas cutscene_CANVAS;
    [SerializeField] TextMeshProUGUI dialogue_TMP;
    [SerializeField] TextEffect dialogue_EFFECT;
    [SerializeField] Image canContinue_IMAGE;
    [SerializeField] Image textBox_IMAGE;
    [SerializeField] Image scene_IMAGE;
    [SerializeField] Image paperBackground_IMAGE;
    [SerializeField] RectTransform DialogueBoxContainer; // Parent for continue icon, text, and visuals | Used for lerping

    [Header("Dialogue Effects")]
    [SerializeField] List<string> appearEffects;
    [SerializeField] List<string> disappearEffects;

    [SerializeField] CutsceneSequence startingCutscene;

    [Header("Scene Global Properties")]
    [SerializeField] float timeTillCanContinue = 1f;

    [Header("Dialogue Box Animation")]
    [SerializeField] bool localMove;
    [SerializeField] float dialogueAppearTime = .25f;
    [SerializeField] float onScreenYPosition;
    [SerializeField] float offScreenYPosition;
    [SerializeField] Ease dialogueAppear_EASE;
    [SerializeField] Ease dialogueDisappear_EASE;

    static bool hasPlayedOpening = false;

    [SerializeField] GameObject player;

    bool CanContinueToNextLine
    {
        get => canContinueToNextLine;
        set
        {
            canContinueToNextLine = value;
            canContinue_IMAGE.enabled = value;
        }
    }

    bool canContinueToNextLine;

    CutsceneSequence currentSequence;

    int sceneIndex;

    public static EventHandler<StateChangedEventArgs> ChangedStateEventHandler;

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

    CutsceneScene CurrentScene => currentSequence.Scenes[sceneIndex];

    void Awake()
    {
        ServiceLocator.ProvideCutscenesService(this);
    }

    void Start()
    {
        Reset();
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
#if DEBUG
        if (Input.GetKeyDown(KeyCode.J))
            SetTextVisibility(true);
        if (Input.GetKeyDown(KeyCode.K))
            SetTextVisibility(false);

#endif
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
        player.GetComponent<PlayerController>().disableMovement = true;
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
        else
            SetTextVisibility(CurrentScene.HasText);

        if (scene.HasNewImage)
            scene_IMAGE.sprite = scene.SceneImage;

        
        paperBackground_IMAGE.color = scene.BackgroundColor == default ? currentSequence.DefaultBackgroundColor : scene.BackgroundColor;
        paperBackground_IMAGE.SetAlpha(1);

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


        float targetAlpha = visible ? 1 : 0;
        float targetPosition = visible ? onScreenYPosition : offScreenYPosition;
        float targetDuration = visible ? dialogueAppearTime : dialogueAppearTime;
        Ease targetEase = visible ? dialogueAppear_EASE : dialogueDisappear_EASE;
        // textBox_IMAGE.enabled = visible;
        // dialogue_TMP.DOFade(targetAlpha, dialogueAppearTime);

        if (localMove)
            DialogueBoxContainer.DOLocalMoveY(targetPosition, targetDuration).SetEase(targetEase);
        else
            DialogueBoxContainer.DOMoveY(targetPosition, targetDuration).SetEase(targetEase);
    }


    bool isWaitingToPlayText;
    void StartDisplayingText(CutsceneScene scene)
    {
        // Finish hiding previous text before playing text
        // Resume execution on animation finish -> OnFinishDisappearAnimation()
        if (sceneIndex != 0 && currentSequence.Scenes[sceneIndex - 1].HasText) // did previous scene have text?
        {
            Debug.Log("wait for animation");

            foreach (string disappearEffect in disappearEffects)
                dialogue_EFFECT.StartManualEffect(disappearEffect);
            isWaitingToPlayText = true;
            return;
        }
        else
        {
            Debug.Log("Play immediately ");
            PlayText(scene.Text);
        }
    }

    void PlayText(string text)
    {
        float alpha = dialogue_TMP.alpha;

        dialogue_TMP.color = CurrentScene.Color;
        dialogue_TMP.alpha = alpha;

        foreach (string effect in appearEffects)
            dialogue_EFFECT.StartManualEffect(effect);

        dialogue_TMP.text = text;
        Debug.Log($"Set dialogue text. {dialogue_TMP.text} | {text}");

        SetTextVisibility(true);
    }

    public void OnFinishTextDisappearAnimation()
    {
        // We don't want to throw an error because you can force exit cutscenes with escape
        if (currentSequence == null)
        {
            Debug.LogWarning("Cutscene sequence became null mid conversation");
            return;
        }
        if (isWaitingToPlayText)
            PlayText(currentSequence.Scenes[sceneIndex].Text);
        isWaitingToPlayText = false;
    }

    void ExitCutscene()
    {
        TextAsset dialogueToPlay = currentSequence != null && currentSequence.DialogueToPlayOnEnd != null ? currentSequence.DialogueToPlayOnEnd : null;
        player.GetComponent<PlayerController>().disableMovement = false;

        OnStateChanged(StateChange.Exited);
        Reset();

        if (dialogueToPlay != null)
            ServiceLocator.GetDialogueService().PlayDialogue(dialogueToPlay);
    }

    void Reset()
    {
        sceneIndex = -1;
        currentSequence = null;
        cutscene_CANVAS.enabled = false;
    }

    void OnStateChanged(StateChange myStateChange)
    {
        ChangedStateEventHandler?.Invoke(this, new StateChangedEventArgs(currentSequence, myStateChange));
    }
}
