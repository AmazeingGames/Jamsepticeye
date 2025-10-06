using UnityEngine;

public interface ISceneHelperService
{
    // Enable a scene's root while disabling all others
    void EnableOnlyTargetScene(string sceneName);
}
