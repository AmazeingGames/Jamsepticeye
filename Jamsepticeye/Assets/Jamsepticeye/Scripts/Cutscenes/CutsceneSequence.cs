using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CutsceneSequence")]
public class CutsceneSequence : ScriptableObject
{
    public enum Cutscene { NotSet, BakerMagic, OpeningSequence }

    [field: SerializeField] public Cutscene MyCutscene { get; private set; }
    [field: SerializeField] public List<CutsceneScene> Scenes { get; private set; }
    [field: SerializeField] public TextAsset DialogueToPlayOnEnd {  get; private set; }

    public int musicIndexForCutscene;
}
