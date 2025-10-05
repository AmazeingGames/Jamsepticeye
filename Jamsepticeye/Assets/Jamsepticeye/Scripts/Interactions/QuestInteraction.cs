using Unity.VisualScripting;
using UnityEngine;

public class QuestInteraction : MonoBehaviour, IInteractable
{
    GameObject IInteractable.Icon { get => interactIcon; }

    [SerializeField] GameObject interactIcon;

    [SerializeField]
    protected GameState[] requiredSetGameStates;

    [SerializeField]
    protected GameState[] requiredUnsetGameStates;

    [SerializeField]
    protected GameState[] addedGameStates;

    [SerializeField]
    protected GameState[] removedGameStates;


    [SerializeField]
    private bool turnInvisibleWhenDone = false;

    [SerializeField]
    private bool enableInteractionsAtTheStart = true; // If true, the object is interactable from the start

    private bool interactionsEnabled;

    [SerializeField]
    private DialogueInteraction dialogueInteraction;

    public void Start()
    {
        interactionsEnabled = enableInteractionsAtTheStart;

        if (interactIcon != null)
            interactIcon.SetActive(false);
    }

    // The game is in the wrong state, process different dialogues based on what state is wrong
    protected virtual void DialogueWrongState() { }

    // The game is in the right state, process successful dialogue
    protected virtual void DialogueRightState() {}

    protected virtual void TriggerSuccess() {}

    void IInteractable.Interact()
    {
        foreach (GameState state in requiredSetGameStates)
        {
            if (!GameStateScript.instance.Is(state))
            {
                DialogueWrongState();
                return;
            }
        }
        foreach (GameState state in requiredUnsetGameStates)
        {
            if (GameStateScript.instance.Is(state))
            {
                DialogueWrongState();
                return;
            }
        }

        Debug.Log($"Trigger Interaction with {gameObject.name}");

        //dialogueInteraction.Interact();
        TriggerSuccess();
        DialogueRightState();

        // Handle game states
        foreach (GameState state in addedGameStates)
            GameStateScript.instance.Set(state);

        foreach (GameState state in removedGameStates)
            GameStateScript.instance.Unset(state);

        if (turnInvisibleWhenDone)
        {
            interactionsEnabled = false;
            gameObject.SetActive(false);
        }
    }
}
