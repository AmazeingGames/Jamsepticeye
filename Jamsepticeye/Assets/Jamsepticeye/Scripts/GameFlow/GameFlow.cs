using System;
using UnityEngine;

public class GameFlow : MonoBehaviour, IGameFlowService
{
    public static EventHandler<EndedGameEventArgs> EndedGameEventHandler;

    public class EndedGameEventArgs : EventArgs { public EndedGameEventArgs() { } }

    void OnEndedGame() { EndedGameEventHandler?.Invoke(this, new EndedGameEventArgs()); }

    public void EndGame()
    {
        OnEndedGame();
    }
}
