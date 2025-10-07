using FMODUnity;
using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SceneRoot : MonoBehaviour
{
    [field: SerializeField] SceneRootData RootData { get; set; }
    [field: SerializeField] Tilemap groundTilemap { get; set; }
    [field: SerializeField] Tilemap groundOverlayTilemap { get; set; }

    public static EventHandler<SetRootActiveEventArgs> SettingActiveEventHandler;

    public class SetRootActiveEventArgs : EventArgs
    {
        public readonly bool isActive;
        public readonly SceneRootData rootData;
        public SetRootActiveEventArgs(SceneRootData rootData, bool isActive)
        {
            this.isActive = isActive;
            this.rootData = rootData;
        }
    }


    private void OnValidate()
    {
        if (groundTilemap == null)
            Debug.LogError("Ground tilemap should not be null");

        if (groundOverlayTilemap == null)
            Debug.LogError("Ground overlay tilemap should not be null");
    }

    public void OnEnable()
    {
        SettingActiveEventHandler?.Invoke(this, new(RootData, true));
    }

    private void OnDisable()
    {
        SettingActiveEventHandler?.Invoke(this, new(RootData, false));
    }
}
