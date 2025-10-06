using UnityEngine;

public class CutsceneTriggerer : MonoBehaviour
{
    [SerializeField] CutsceneSequence startingSequence;
    [SerializeField] CutsceneSequence bakerMagicSequence;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startingSequence != null)
            ServiceLocator.GetCutscenesService().TriggerCutsceneSequence(startingSequence);
    }
}
