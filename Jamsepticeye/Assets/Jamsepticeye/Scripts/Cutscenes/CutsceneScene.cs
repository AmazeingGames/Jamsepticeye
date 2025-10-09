using System;
using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

[Serializable]
public struct CutsceneScene
{
    [field: SerializeField] public Sprite SceneImage { get; private set; }
    [field: SerializeField] public string Text {get; private set; }
    [field: SerializeField] public Color Color { get; private set; }
    [field: SerializeField] public Color BackgroundColor { get; private set; }


    public readonly bool HasNewImage { get => SceneImage != null; }
    public readonly bool HasText { get => Text != string.Empty && Text != "" && Text != " "; }
}
