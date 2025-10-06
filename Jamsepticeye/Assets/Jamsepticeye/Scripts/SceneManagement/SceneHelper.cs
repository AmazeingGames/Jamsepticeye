using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VInspector;

public class SceneHelper : MonoBehaviour, ISceneHelperService
{
    List<GameObject> sceneRootObjects = new();

    void Awake()
        => ServiceLocator.ProvideSceneHelperService(this);

    private void OnEnable()
        => SceneManager.sceneLoaded += HandleSceneLoaded;
    
    private void OnDisable()
        => SceneManager.sceneLoaded -= HandleSceneLoaded;
    
    public void EnableOnlyTargetScene(string sceneName)
        => EnableSceneRoot(sceneName);

    void HandleSceneLoaded(Scene scene, LoadSceneMode myLoadSceneMode)
        => EnableSceneRoot(scene);

    void EnableSceneRoot(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name == sceneName)
            {
                EnableSceneRoot(scene);
                return;
            }
        }
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    void EnableSceneRoot(Scene scene)
    {
        foreach (GameObject root in sceneRootObjects)
            root.SetActive(false);

        foreach (GameObject gameObject in scene.GetRootGameObjects())
        {
            Debug.Log("loop");
            if (gameObject.name == "Root")
            {
                gameObject.SetActive(true);
                sceneRootObjects.Add(gameObject);
                return;
            }
        }
        throw new System.Exception("Scene does not have root gameobjct");
    }
}
