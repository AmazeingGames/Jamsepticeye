using UnityEngine;

public class PeepScript : MonoBehaviour
{
    void Update()
    {
        if (GameStateScript.Instance.Is(GameState.PEEP_POOFED) && !GameStateScript.Instance.Is(GameState.END_SCENE_SETUP_DONE))
            GetComponent<SpriteRenderer>().enabled = false;
    }
}
