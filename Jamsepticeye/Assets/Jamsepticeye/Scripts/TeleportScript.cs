using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportScript : MonoBehaviour
{

    [SerializeField]
    private string sceneDestination;

    public void Teleport()
    {
        SceneManager.LoadSceneAsync(sceneDestination);
    }
}
