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

    bool canContinueToNextLine = false;

    static DialogueManager instance;

    DialogueVariables dialogueVariables;
    InkExternalFunctions inkExternalFunctions;

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
            )
            ContinueStory();
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
    }

    void ContinueStory() 
    {
        if (!currentStory.canContinue)
            StartCoroutine(ExitDialogueMode());

        string nextLine = currentStory.Continue();

        // handle case where the last line is an external function
        if (nextLine.Equals("") && !currentStory.canContinue)
        {
            Debug.Log("Last line is external function");
            StartCoroutine(ExitDialogueMode());
        }
        // Otherwise, handle the normal case for continuing the story
        else 
        {
            HandleTags(currentStory.currentTags);
            DisplayLine(nextLine);
        }
        
    }

    void DisplayLine(string line) 
    {
        dialogue_TMP.text = line;
        foreach (string effect in appearEffects)
            dialogue_EFFECT.StartManualEffect(effect);

        // hide items while text is typing
        HideChoices();
        CanContinueToNextLine = false;
    }

    public void OnFinishTextAnimation()
    {
        Debug.Log("Line Completed");

        DisplayChoices();
        CanContinueToNextLine = true;
    }

    void HideChoices()  
    {
        foreach (GameObject choiceButton in choices) 
            choiceButton.SetActive(false);
    }

    public void HideText()
    {

    }

    void DisplayChoices() 
    {
        // foreach (string disappearEffect in disappearEffects)
        // dialogue_EFFECT.StartManualEffect(disappearEffect);

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

        StartCoroutine(SelectFirstChoice());
    }

    IEnumerator SelectFirstChoice() 
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0]);
    }

    public void MakeChoice(int choiceIndex)
    {
        if (CanContinueToNextLine) 
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            // NOTE: The below two lines were added to fix a bug after the Youtube video was made
            InputManager.GetInstance().RegisterSubmitPressed(); // this is specific to my InputManager script
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
