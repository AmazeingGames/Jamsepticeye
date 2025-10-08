using FMODUnity;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SceneRoot : MonoBehaviour
{
    [field: SerializeField] SceneRootData RootData { get; set; }
    [field: SerializeField] bool hasNoTilemaps; // move this to root data


    [HideIf("hasNoTilemaps")]
    [SerializeField] public Tilemap groundOverlayTilemap;
    [HideIf("hasNoTilemaps")]
    [SerializeField] public Tilemap groundTilemap;


    public static EventHandler<EnablingRootEventArgs> EnablingRootEventHandler;

    [SerializeField] float timeToWaitForScenesToLoad_SECONDS = .1f;
    static bool hasLoadedRootBefore = false;

    public class EnablingRootEventArgs : EventArgs
    {
        public readonly bool isSettingActive;
        public readonly SceneRootData rootData;
        public readonly Tilemap groundOverlayTilemap;
        public readonly Tilemap groundTilemap;

        public EnablingRootEventArgs(SceneRootData rootData, bool isActive, Tilemap groundOverlayTilemap, Tilemap groundTilemap)
        {
            this.isSettingActive = isActive;
            this.rootData = rootData;
            this.groundOverlayTilemap = groundOverlayTilemap;
            this.groundTilemap = groundTilemap;
        }
    }

    private void OnValidate()
    {
        if (hasNoTilemaps)
            return;

        if (groundTilemap == null)
            Debug.LogError("Ground tilemap not set on Root gameobject in the hierarchy");

        if (groundOverlayTilemap == null)
            Debug.LogError("Ground overlay tilemap not set on Root gameobject in the hierarchy");
    }

    public void OnEnable()
    {
        StartCoroutine(OnEnablingRoot_CO(true));
    }

    private void OnDisable()
    {
        // We only need the time delay if we're enabling the game object
        // Ignore suggestion; calling with StartCoroutine produces an exception due to object being disabled
        OnEnablingRoot_CO(false);
    }

    IEnumerator OnEnablingRoot_CO(bool setActive)
    {
        Debug.Log("Enabling root");
        // Wait for everything in the scene to finish loading
        if (!hasLoadedRootBefore)
        {
            hasLoadedRootBefore = true;
            yield return new WaitForSeconds(timeToWaitForScenesToLoad_SECONDS);
        }
        EnablingRootEventHandler?.Invoke(this, new(RootData, setActive, groundOverlayTilemap, groundTilemap));
    }
}

        
