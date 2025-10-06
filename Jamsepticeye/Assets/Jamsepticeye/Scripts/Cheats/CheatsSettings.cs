using MoreMountains.Tools;
using UnityEngine;

public class CheatsSettings : MMSingleton<CheatsSettings>
{
    [field: SerializeField] public bool CheatGameStateOnStart { get; private set; }
}
