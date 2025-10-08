using UnityEngine;
using UnityEngine.UIElements;

public enum GameState : int
{
    PLACED_HAMMOCK = 0x01,
    HAS_EGGS = 0x02,
    NEEDS_SUGAR = 0x04,
    KNOWS_ABOUT_BAKER = 0x08,
    TALKED_TO_BAKER = 0x10, 
    NEEDS_EGGS = 0x20,
    GAVE_INGREDIENTS_TO_BAKER = 0x40,
    HAS_ROCKS = 0x80,
    MURDERED_BAKER = 0x100,
    HAS_COOKIES = 0x200,
    FOUND_NEST = 0x400,
    NEEDS_STICKS = 0x800,
    NEEDS_ROCKS = 0x1000,
    HAS_SUGAR = 0x2000,
    HAS_COFFEE = 0x4000,
    KID_FED = 0x8000,
    HAS_STICKS = 0x10000,
    NEST_ROCKING_STARTS = 0x20000,
    BAKER_DEAD = 0x40000,
    ALLOWED_BAKERY = 0x80000,
    FLOUR_MAGIC_READY = 0x100000,
    ROCK_THROWN = 0x200000,
    KID_CHOKING = 0x400000,
    COOKIES_BAKED = 0x800000,
    KID_CHOKING_DIALOG = 0x1000000,
    HAMMOCK_FADE_STARTED = 0x2000000,
    END_SCENE_SETUP = 0x4000000,
    END_SCENE_SETUP_DONE = 0x8000000,
    PEEP_POOFED = 0x10000000,
};

public class GameStateScript
{
    [SerializeField]
    private GameState gameState;

    private static GameStateScript _instance;

    public static GameStateScript Instance
    {
        get
        {
            _instance ??= new GameStateScript();
            return _instance;
        }
    }

    private GameStateScript()
    {
        // Initialize our game state with the correct state
        Set(GameState.NEEDS_ROCKS);
        Set(GameState.NEEDS_STICKS);

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
