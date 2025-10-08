using System;
using System.Collections;
using UnityEngine;

public class GameFlow : MonoBehaviour, IGameFlowService
{
    public static EventHandler<EndingGameEventArgs> EndingGameEventHandler;

    public class EndingGameEventArgs : EventArgs { public EndingGameEventArgs() { } }

    [SerializeField] float timeForFadeOut = 2;

    void Awake()
    {
        ServiceLocator.ProvideGameFlowService(this);
    }

    void Update()
    {
        Debug.Log("running");
# if DEBUG
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("end t");
            EndGame();

        }
#endif
    }

    IEnumerator OnEndedGame_CO() 
    { 
        yield return new WaitForSeconds(timeForFadeOut);

        EndingGameEventHandler?.Invoke(this, new EndingGameEventArgs());
        Debug.Log("end game");
        ServiceLocator.GetSceneHelperSerivce().EnableOnlyTargetScene("EndGame");
    }

    public void EndGame()
    {
        Debug.Log("end here");
        StartCoroutine(OnEndedGame_CO());
    }
}
