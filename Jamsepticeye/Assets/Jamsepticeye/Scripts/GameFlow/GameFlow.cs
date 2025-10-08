using UnityEngine;

public class GameFlow : MonoBehaviour, IGameFlowService
{

    public void EndGame()
    {
        ServiceLocator.GetSceneHelperSerivce().EnableOnlyTargetScene("GameEnd");
    }
}
