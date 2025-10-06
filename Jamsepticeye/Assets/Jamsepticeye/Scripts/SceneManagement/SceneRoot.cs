using FMODUnity;
using System;
using UnityEditor.Rendering;
using UnityEngine;

public class SceneRoot : MonoBehaviour
{
    [field: SerializeField] SceneRootData RootData { get; set; }

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


    public void OnEnable()
    {
        SettingActiveEventHandler?.Invoke(this, new(RootData, true));
    }

    private void OnDisable()
    {
        SettingActiveEventHandler?.Invoke(this, new(RootData, false));
    }
}
