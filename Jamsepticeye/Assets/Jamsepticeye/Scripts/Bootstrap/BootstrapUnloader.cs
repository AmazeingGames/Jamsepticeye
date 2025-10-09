using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapUnloader : MonoBehaviour
{
    void OnEnable()
    {
        SceneRoot.EnablingRootEventHandler += Scenes_EnablingRoot;
    }

    void OnDisable()
    {
        SceneRoot.EnablingRootEventHandler += Scenes_EnablingRoot;
    }

    void Scenes_EnablingRoot(object sender, SceneRoot.EnablingRootEventArgs e)
    {
        switch (e.rootData.MyScene)
        {
            case SceneRootData.SceneType.None:
                break;
            case SceneRootData.SceneType.Village:
                SceneManager.UnloadSceneAsync("Bootstrapper");
                break;
            case SceneRootData.SceneType.Bakery:
                break;
            case SceneRootData.SceneType.GroceryStore:
                break;
            case SceneRootData.SceneType.Menu:
                break;
            case SceneRootData.SceneType.Bootstrap:
                break;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
