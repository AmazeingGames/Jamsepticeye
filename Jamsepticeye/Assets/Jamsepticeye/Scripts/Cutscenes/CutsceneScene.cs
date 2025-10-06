using System;
using UnityEngine;
using VInspector;

[Serializable]
public struct CutsceneScene
{
    // change
    [SerializeField] public Animation EntryAnimation { get; private set; }
    [field: SerializeField] public Sprite SceneImage { get; private set; }
    [field: SerializeField] public string Text {get; private set; }
    [field: SerializeField] public Color Color { get; private set; }

    // Change to FMOD sounds
    [SerializeField] public string EntrySFX {get; private set; }

    public bool HasNewImage { get => SceneImage != null; }
    public bool ShouldHideText { get => Text == string.Empty; }
}
