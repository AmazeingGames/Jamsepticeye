using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine;

public class CheatsSettings : MMSingleton<CheatsSettings>
{
    public enum CheatType { None, CanDoBakerQuest, CanBuildHammock }
    [SerializeField] CheatType myCheatType;

    GameStateScript State => GameStateScript.Instance;

    List<GameState> cheatedStates = new List<GameState>();

    private void Update()
    {
        cheatedStates.Clear();

# if DEBUG
        switch (myCheatType)
        {
            case CheatType.CanDoBakerQuest:
                cheatedStates.Add(GameState.KNOWS_ABOUT_BAKER);
                cheatedStates.Add(GameState.TALKED_TO_BAKER);
                cheatedStates.Add(GameState.HAS_SUGAR);
                cheatedStates.Add(GameState.HAS_EGGS);
                break;

            case CheatType.CanBuildHammock:
                cheatedStates.Add(GameState.HAS_ROCKS);
                cheatedStates.Add(GameState.HAS_STICKS);
                cheatedStates.Add(GameState.NEEDS_EGGS);
                cheatedStates.Add(GameState.KNOWS_ABOUT_BAKER);
                cheatedStates.Add(GameState.TALKED_TO_BAKER);
                cheatedStates.Add(GameState.NEEDS_SUGAR);
                cheatedStates.Add(GameState.PEEP_POOFED);
                break;

            case CheatType.None:
                foreach (GameState cheatedState in cheatedStates)
                    State.Unset(cheatedState);
                break;
        }
        foreach (GameState cheatedState in cheatedStates)
            State.Set(cheatedState);
    }
#endif
}