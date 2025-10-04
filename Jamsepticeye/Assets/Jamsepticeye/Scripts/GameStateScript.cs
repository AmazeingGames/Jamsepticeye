using UnityEngine;
using UnityEngine.UIElements;

public enum GameState : int
{
    PLACED_HAMMOCK = 0x01,
    HAS_MONEY_FROM_BAKER = 0x02,
    HAS_SUGAR = 0x04,
    KNOWS_ABOUT_BAKER = 0x08,
    HAS_STICKS = 0x10,
    HAS_EGGS = 0x20,
    GAVE_INGREDIENTS_TO_BAKER = 0x40,
    MURDERED_BAKER = 0x80,
    HAS_COOKIES = 0x100
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
        instance = this;
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
