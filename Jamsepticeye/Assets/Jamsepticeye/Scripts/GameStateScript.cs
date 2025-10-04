using UnityEngine;
using UnityEngine.UIElements;

public enum GameState : int
{
    PLACED_HAMMOCK = 0x01,
    HAS_MONEY_FROM_BAKER = 0x02,
    HAS_SUGAR = 0x04,
    KNOWS_ABOUT_BAKER = 0x08,
    TALKED_TO_BAKER = 0x10,
    HAS_STICKS = 0x20,
    HAS_EGGS = 0x40,
    GAVE_INGREDIENTS_TO_BAKER = 0x80,
    MURDERED_BAKER = 0x100,
    HAS_COOKIES = 0x200,
    FOUND_NEST = 0x400,
    NEEDS_STICKS = 0x800,
    HAS_ROCKS = 0x1000,
    NEEDS_ROCKS = 0x2000
};

public class GameStateScript : MonoBehaviour
{
    [SerializeField]
    GameState[] testingGameStates; // Only for testing purposes

    [SerializeField]
    private GameState gameState;

    public static GameStateScript instance { get; private set; }

    private void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            // If another instance exists, destroy this one
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        foreach (GameState state in testingGameStates)
        {
            gameState |= state;
        }
        if (testingGameStates.Length == 0)
        {
            // Initialize our game state with the correct state
        }
    }
    public bool Is(GameState state)
    {
        return (state & gameState) == state;
    }
    public void Set(GameState state)
    {
        gameState |= state;
    }
    public void Unset(GameState state)
    {
        gameState &= ~state;
    }
}
