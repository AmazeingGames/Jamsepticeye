using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CutsceneSequence")]
public class CutsceneSequence : ScriptableObject
{
    [field: SerializeField] public List<CutsceneScene> Scenes { get; private set; }
    [field: SerializeField] public TextAsset TextDialog { get; private set; }

    public int musicIndexForCutscene;
}
