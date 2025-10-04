using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportScript : MonoBehaviour
{
    [SerializeField]
    private string sceneDestination;

    [SerializeField]
    private Vector2 spawnPoint;

    public void Teleport()
    {
        var oldScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneDestination);
        SpawnPointHandler.TeleportToScene(spawnPoint);
    }
}
