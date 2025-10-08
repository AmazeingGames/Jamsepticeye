using UnityEngine;

public class PeepScript : MonoBehaviour
{
    void Update()
    {
        if (GameStateScript.Instance.Is(GameState.PEEP_POOFED))
            GetComponent<SpriteRenderer>().enabled = false;
    }
}
