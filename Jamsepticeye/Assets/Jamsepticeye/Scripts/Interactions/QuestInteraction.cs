using Unity.VisualScripting;
using UnityEngine;

public class QuestInteraction : MonoBehaviour, IInteractable
{
    GameObject IInteractable.Icon { get => interactIcon; }
    bool IInteractable.Enabled { get => interactionsEnabled; }

    [SerializeField] GameObject interactIcon;

    [SerializeField]
    private GameState[] requiredGameStates;

    [SerializeField]
    private GameState[] addedGameStates;

    [SerializeField]
    private GameState[] removedGameStates;


    [SerializeField]
    private bool turnInvisibleWhenDone = false;

    [SerializeField]
    private bool enableInteractionsAtTheStart = true; // If true, the object is interactable from the start

    private bool interactionsEnabled;
    void Start()
    {
        interactionsEnabled = enableInteractionsAtTheStart;

        interactIcon.SetActive(false);
        if (addedGameStates.Length == 0 && removedGameStates.Length == 0)
        {
            // Quest interactions are supposed to progress the story, the game state should evolve.
            Debug.LogWarning($"Make sure to set game states for interactions {gameObject.name}");
        }
    }

    // The game is in the wrong state, process different dialogues based on what state is wrong
    void DialogueWrongState()
    {
    }

    // The game is in the right state, process successful dialogue
    void DialogueRightState()
    {

    }
    void IInteractable.Interact()
    {
        foreach (GameState state in requiredGameStates)
        {
            if (!GameStateScript.instance.Is(state))
            {
                DialogueWrongState();
                return;
            }
        }

        Debug.Log($"Trigger Interaction with {gameObject.name}");

        // TODO: Trigger optional dialogue
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
