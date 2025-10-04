using System;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

[Serializable]
public struct Speaker 
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public SerializedDictionary<string, Sprite> EmotionToSprite { get; private set; }
}
