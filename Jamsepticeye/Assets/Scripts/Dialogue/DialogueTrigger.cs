using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] GameObject visualCue;
    [SerializeField] Animator emoteAnimator;
    [SerializeField] TextAsset inkJSON;

    private bool playerInRange;

    private void Awake() 
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update() 
    {
        if (playerInRange && !DialogueManager.GetInstance().IsDialoguePlaying) 
        {
            visualCue.SetActive(true);
            if (InputManager.GetInstance().GetInteractPressed()) 
                DialogueManager.GetInstance().PlayDialogue(inkJSON);
        }
        else 
            visualCue.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collider) 
    {
        if (collider.gameObject.CompareTag("Player"))
            playerInRange = false;
    }
}
