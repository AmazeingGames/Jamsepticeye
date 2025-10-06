using EasyTextEffects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

// So much duplication with the dialogue manager indicates some refactoring should be done
public class CutscenesPlayer : MonoBehaviour, ICutscenesService
{
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
        ExitCutscene();
    }

    [SerializeField] float spamPreventionTimer = .2f;

    bool isStoppingSpam;
    float timeSinceLastSpacePress;
    private void Update()
    {
        timeSinceLastSpacePress += Time.deltaTime;

        if (timeSinceLastSpacePress > spamPreventionTimer)
            isStoppingSpam = false;

        if (isStoppingSpam)
        {
            // Debug.Log("Stopping Spam");
            return;
        }

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
        cutscene_CANVAS.enabled = true;
        currentSequence = cutsceneSequence;
        // RuntimeManager.StudioSystem.setParameterByName("MusicType", cutsceneSequence.musicIndexForCutscene);
        sceneIndex = -1;

        DisplayNextScene();
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

        if (scene.ShouldHideText)
            HideText();
        else
            StartDisplayingText(scene);

        if (scene.HasNewImage)
            scene_IMAGE.sprite = scene.SceneImage;

        if (scene.EntrySFX != "")
            Debug.Log($"Play sfx {scene.EntrySFX}");

        if (scene.EntryAnimation != null)
            Debug.Log($"Play entry animation {scene.EntryAnimation}");
    }

    IEnumerator StartSceneEnd_CO()
    {
        yield return new WaitForSeconds(timeTillCanContinue);
        CanContinueToNextLine = true;
    }

    void HideText()
    {
        textBox_IMAGE.enabled = false;
        dialogue_TMP.text = "";
    }

    bool isWaitingToPlayText;
    void StartDisplayingText(CutsceneScene scene)
    {
        // Finish hiding previous text before playing text
        // Resume execution on animation finish
        if (sceneIndex != 0 && !currentSequence.Scenes[sceneIndex - 1].ShouldHideText) // did previous scene have text?
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

        foreach (string effect in appearEffects)
            dialogue_EFFECT.StartManualEffect(effect);

        dialogue_TMP.enabled = true;
    }

    public void OnFinishTextDisappearAnimation()
    {
        if (isWaitingToPlayText)
        {
            PlayText(currentSequence.Scenes[sceneIndex].Text);
        }
    }

    void ExitCutscene()
    {
        if (currentSequence != null && currentSequence.DialogueToPlayOnEnd != null)
            ServiceLocator.GetDialogueService().PlayDialogue(currentSequence.DialogueToPlayOnEnd);

        currentSequence = null;
        sceneIndex = -1;
        cutscene_CANVAS.enabled = false;
    }
}
