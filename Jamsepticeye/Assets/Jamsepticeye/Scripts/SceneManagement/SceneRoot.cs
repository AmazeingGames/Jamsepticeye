using FMODUnity;
using Sirenix.OdinInspector;
using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SceneRoot : MonoBehaviour
{
    [field: SerializeField] SceneRootData RootData { get; set; }
    [field: SerializeField] bool hasNoTilemaps;

    [HideIf("hasNoTilemaps")]
    [SerializeField] public Tilemap groundOverlayTilemap;
    [HideIf("hasNoTilemaps")]
    [SerializeField] public Tilemap groundTilemap;


    public static EventHandler<EnablingRootEventArgs> EnablingRootEventHandler;

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
        EnablingRootEventHandler?.Invoke(this, new(RootData, true, groundOverlayTilemap, groundTilemap));
    }

    private void OnDisable()
    {
        EnablingRootEventHandler?.Invoke(this, new(RootData, false, groundOverlayTilemap, groundTilemap));
    }
}
