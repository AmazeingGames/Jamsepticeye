using EasyTextEffects;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour, IDialogueService
{
    [Header("Load Globals JSON")]
    [SerializeField] TextAsset loadGlobalsJSON;

    [Header("Dialogue UI")]
    [SerializeField] Canvas dialogue_CANVAS;
    [SerializeField] TextMeshProUGUI dialogue_TMP;
    [SerializeField] TextEffect dialogue_EFFECT;
    [SerializeField] TextMeshProUGUI speakerName_TMP;
    [SerializeField] Image canContine_IMAGE;
    [SerializeField] Image speaker_IMAGE;

    [Header("Dialogue Effects")]
    [SerializeField] List<string> appearEffects;
    [SerializeField] List<string> disappearEffects;

    [Header("Choices UI")]
    [SerializeField] GameObject choicesParent;
    List<GameObject> choices = new();

    [Header("Speakers")]
    [SerializeField] Speaker bjorn;
    [SerializeField] Speaker peeper;
    [SerializeField] Speaker tim;

    Dictionary<string, Speaker> speakerNameToData;

    Speaker currentSpeaker;

    List<TextMeshProUGUI> choicesText;

    Story currentStory;

    public bool IsDialoguePlaying { get; set; }

    bool CanContinueToNextLine 
    {
        get => canContinueToNextLine;
        set
        {
            canContinueToNextLine = value;
            canContine_IMAGE.enabled = value;
        }
    }

    bool isSelectingChoice;
    public bool canContinueToNextLine = false;

    static DialogueManager instance;

    DialogueVariables dialogueVariables;
    InkExternalFunctions inkExternalFunctions;
    private bool isDialogueDisappearing;

    void Awake() 
    {
        speakerNameToData = new()
        {
            { "baker", bjorn },
            { "peeper", peeper },
            { "tim", tim },
        };

        ServiceLocator.ProvideService(this);

        if (instance != null)
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");

        instance = this;

        dialogueVariables = new DialogueVariables(loadGlobalsJSON);
        inkExternalFunctions = new InkExternalFunctions();
    }

    public static DialogueManager GetInstance() 
        => instance;

    void Start() 
    {
        IsDialoguePlaying = false;
        dialogue_CANVAS.gameObject.SetActive(false);

        // get all of the choices text 
        choicesText = new();

        for (int i = 0; i < choicesParent.transform.childCount; i++)
        {
            var choice = choicesParent.transform.GetChild(i).gameObject;
            choicesText.Add(choice.GetComponentInChildren<TextMeshProUGUI>());

            choices.Add(choice);
        }
    }


    void Update() 
    {
        // return right away if dialogue isn't playing
        if (!IsDialoguePlaying) 
            return;

        Assert.IsNotNull(currentStory, "Current story shoud not be null");
        Assert.IsNotNull(currentStory.currentChoices, "Current choices shoud not be null");

        // handle continuing to the next line in the dialogue when submit is pressed
        if (
            CanContinueToNextLine 
            && currentStory.currentChoices.Count == 0 
            && Input.GetButtonDown("Continue")
            && !isDialogueDisappearing)
            ContinueStory();

        else if (
            CanContinueToNextLine
            && currentStory.currentChoices.Count != 0
            && Input.GetButtonDown("Continue")
            && !isSelectingChoice)
        {
            Debug.Log("start displaying choices");
            // Once the text is finished disappearing, we call DisplayChoices()
            isDialogueDisappearing = true;
            foreach (string disappearEffect in disappearEffects)
                dialogue_EFFECT.StartManualEffect(disappearEffect);
        }
    }

    public void PlayDialogue(TextAsset inkJSON) 
    {
        Assert.IsNotNull(inkJSON, "Conversation is not set by the interactable entity.");
        Debug.Log("Play dialogue");

        // Staging
        currentStory = new Story(inkJSON.text);
        IsDialoguePlaying = true;
        dialogue_CANVAS.gameObject.SetActive(true);
        
        dialogueVariables.StartListening(currentStory);
        inkExternalFunctions.BindEmoteFunction(currentStory);

        ContinueStory();
    }

    IEnumerator ExitDialogueMode() 
    {
        yield return new WaitForSeconds(0.2f);

        dialogueVariables.StopListening(currentStory);
        inkExternalFunctions.UnbindEmote(currentStory);

        IsDialoguePlaying = false;
        dialogue_CANVAS.gameObject.SetActive(false);
        dialogue_TMP.text = "";

        Debug.Log("Exit dialogue");
    }

    void ContinueStory() 
    {
        if (!currentStory.canContinue)
            StartCoroutine(ExitDialogueMode());

        string upcomingLine = currentStory.Continue();

        bool isNextLineExternalFunction = upcomingLine.Equals("") && !currentStory.canContinue;

        if (isNextLineExternalFunction)
        {
            Debug.Log("Last line is external function");
            StartCoroutine(ExitDialogueMode());
            return;
        }

        HandleTags(currentStory.currentTags);
        DisplayLine(upcomingLine);
    }

    void DisplayLine(string line) 
    {
        Debug.Log("Display Line");
        dialogue_TMP.enabled = true;
        dialogue_TMP.text = line;

        foreach (GameObject choiceButton in choices)
            choiceButton.SetActive(false);

        CanContinueToNextLine = false;
        
        // Can only continue to the next once the current line finishes displaying
        // From here, flow of execution is decided in Update to resolve next action
        foreach (string effect in appearEffects)
            dialogue_EFFECT.StartManualEffect(effect);        
    }

    // Called from the inspector when the text finishes displaying
    public void OnFinishTextDisplayAnimation()
    {
        Debug.Log("Line Completed");

        CanContinueToNextLine = true;
    }

    // Called from the inspector when the dialogue disappear animation finishes
    public void DisplayChoices() 
    {
        if (isSelectingChoice)
            return;

        isDialogueDisappearing = false;
        isSelectingChoice = true;
        dialogue_TMP.enabled = false;

        Debug.Log("Display Choices");
        List<Choice> currentChoices = currentStory.currentChoices;

        // defensive check to make sure our UI can support the number of choices coming in
        if (currentChoices.Count > choices.Count)
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " 
                + currentChoices.Count);

        int index = 0;

        // enable and initialize the choices up to the amount of choices for this line of dialogue
        foreach(Choice choice in currentChoices) 
        {
            choices[index].SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Count; i++) 
            choices[i].SetActive(false);

        StartCoroutine(HighlightFirstChoice());
    }

    IEnumerator HighlightFirstChoice() 
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0]);
    }

    // Called from the inspector when we click a choice button
    public void MakeChoice(int choiceIndex)
    {
        isSelectingChoice = false;

        if (CanContinueToNextLine) 
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }

    void HandleTags(List<string> currentTags)
    {
        // loop through each tag and handle it accordingly
        foreach (string tag in currentTags)
        {
            // parse the tag
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            // handle the tag
            switch (tagKey)
            {
                case "speaker":
                    currentSpeaker = speakerNameToData[tagValue];
                    break;

                case "emotion":
                    speaker_IMAGE.sprite = currentSpeaker.EmotionToSprite[tagValue];
                    break;

                case "layout":
                    if (tagValue == "left") { }
                    else if (tagValue == "right") { }
                    break;

                default:
                    Debug.LogWarning("Tag not handled: " + tag);
                    break;
            }
        }
    }

    public Ink.Runtime.Object GetVariableState(string variableName) 
    {
        dialogueVariables.variables.TryGetValue(variableName, out Ink.Runtime.Object variableValue);

        if (variableValue == null) 
            Debug.LogWarning("Ink Variable was found to be null: " + variableName);
        return variableValue;
    }

    // This method will get called anytime the application exits.
    // Depending on your game, you may want to save variable state in other places.
    public void OnApplicationQuit() 
        => dialogueVariables.SaveVariables();
}
