using UnityEngine;
using UnityEngine.EventSystems;

public class PlayButton : MonoBehaviour, IPointerClickHandler
{
    static bool hasStartedGame = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        hasStartedGame = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Because the game doesn't properly reset itself on game end, we might as well not allow the play to enter the game for a second time
        if (hasStartedGame)
            gameObject.SetActive(false);
    }
}
