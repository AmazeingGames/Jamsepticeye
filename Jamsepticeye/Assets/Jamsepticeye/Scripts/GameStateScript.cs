using UnityEngine;
using UnityEngine.UIElements;

public enum GameState : int
{
    HAS_CAPE = 0x01,
    HAS_MONEY_FROM_BAKER = 0x02,
    HAS_SUGAR_IN_INVENTORY = 0x04,
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

            Set(GameState.HAS_CAPE); // Player has his cape at the start
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
