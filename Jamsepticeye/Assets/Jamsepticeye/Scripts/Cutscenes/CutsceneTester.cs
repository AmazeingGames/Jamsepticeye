using UnityEngine;

public class CutsceneTester : MonoBehaviour
{
    [SerializeField] CutsceneSequence testSequence;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if DEBUG
        if (Input.GetKeyDown(KeyCode.M))
            ServiceLocator.GetCutscenesService().TriggerCutsceneSequence(testSequence);

#endif
    }
}
