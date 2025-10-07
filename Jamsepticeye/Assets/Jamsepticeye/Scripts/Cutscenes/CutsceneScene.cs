using System;
using UnityEngine;
using VInspector;

[Serializable]
public struct CutsceneScene
{
    // change
    [field: SerializeField] public Sprite SceneImage { get; private set; }
    [field: SerializeField] public string Text {get; private set; }
    [field: SerializeField] public Color Color { get; private set; }

    // Change to FMOD sounds
    [SerializeField] public string EntrySFX {get; private set; }

    public bool HasNewImage { get => SceneImage != null; }
    [field: SerializeField] public bool HasText { get => Text != string.Empty && Text != "" && Text != " "; }
}
