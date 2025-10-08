using System;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

[Serializable]
// I honestly think with how much is being jammed into the speaker class that it would be better off as a scriptable object in order to better separate the data from the code
public struct Speaker 
{
    /// <summary>
    ///     Note: The names (most likely) cannot match the names within the inky file, so use the name string for that purpose
    /// </summary>
    public enum Character { None, Baker, Peeper, Tim, HungryBoy, DocDoor }

    [field: SerializeField] public Character MySpeaker;
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public SerializedDictionary<string, Sprite> EmotionToSprite { get; private set; }
}
