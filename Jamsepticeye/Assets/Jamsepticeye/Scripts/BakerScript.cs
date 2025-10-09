using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using Ink.Parsed;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static ItemData;
using static GameStateTracker;

public class BakerScript : MonoBehaviour
{
    [SerializeField] CutsceneSequence bakerMagic;

    private Animator animator;

    [SerializeField] PathFollower pathFollower;
    [SerializeField] new PolygonCollider2D collider;
    [SerializeField] GameObject cookies;

    DialogueInteraction dialogueInteraction;
    BoxCollider2D boxCollider;

    [SerializeField]
    List<GameObject> itemsDisappearOnDeath = new();
    [SerializeField]
    List<GameObject> itemsAppearOnDeath = new();

    public enum Action { None, BakingCookies }

    public static EventHandler<PerformingActionEventArgs> PerformingActionEventHandler;

    public class PerformingActionEventArgs : EventArgs 
    {
        public readonly Action myAction;
        public PerformingActionEventArgs(Action myAction) 
            => this.myAction = myAction;
    }

    void OnPerformingAction(Action myAction) 
    { 
        PerformingActionEventHandler?.Invoke(this, new PerformingActionEventArgs(myAction)); 
    }


    public void Start()
    {
        animator = GetComponent<Animator>();
        // Look to the left at the beginning (at the player)
        animator.SetFloat("LookX", -1);
        animator.SetFloat("LookY", 0);

        pathFollower = GetComponentInParent<PathFollower>();
        dialogueInteraction = GetComponentInParent<DialogueInteraction>();
        boxCollider = GetComponentInParent<BoxCollider2D>();

        foreach (var item in itemsAppearOnDeath)
            item.SetActive(false);

        foreach (var item in itemsDisappearOnDeath)
            item.SetActive(true);

    }

    public void Update()
    {
        //if (GameStateScript.Instance.Is(GameState.COOKIES_BAKED) && !GameStateScript.Instance.Is(GameState.HAS_COOKIES))

        if (!DialogueManager.IsDialoguePlaying && IsGameState(NewGameState.CanPerformBakerMagic))
        {
            ServiceLocator.GetCutscenesService().TriggerCutsceneSequence(bakerMagic);

            foreach (var item in itemsAppearOnDeath)
                item.SetActive(true);

            foreach (var item in itemsDisappearOnDeath)
                item.SetActive(false);

            cookies.GetComponent<SimpleInteraction>().enabled_ = true;
            cookies.GetComponent<SimpleInteraction>().InteractIcon.SetActive(true);
            return;
        }
        if (pathFollower != null)
        {
            if (!pathFollower.pathStarted)
            {
                bool hasStartedPathAnimation = !(InventoryDataManager.HasItem(ItemType.Eggs) && InventoryDataManager.HasItem(ItemType.Sugar));
                if (!hasStartedPathAnimation)
                {
                    // Player got through the cutscene and gave his ingredients to the baker. The baker should start moving.
                    pathFollower.StartPath();
                    collider.enabled = false; // No interaction with the trigger zone
                }
            }
        }
        if (dialogueInteraction != null)
        {
            if (pathFollower != null)
            {
                if (!pathFollower.pathComplete && pathFollower.pathStarted)
                {
                    // Baker is moving, no interaction
                    if (boxCollider != null)
                        boxCollider.enabled = false;
                    dialogueInteraction.Disable();
                    return;
                }

                else if (pathFollower.pathComplete && !IsGameState(NewGameState.HasBakedCookies))
                {
                    // He just arrives at his spot
                    StartCoroutine(PutCookiesOnTable());
                    FadeController.instance.TriggerFade();
                }
            }
        }
        if (boxCollider != null)
            boxCollider.enabled = true;
        dialogueInteraction.Enable();
    }

    private IEnumerator PutCookiesOnTable()
    {
        OnPerformingAction(Action.BakingCookies);

        yield return new WaitForSeconds(1f);

        cookies.SetActive(true);
        // Put it on table, not interactable
        cookies.GetComponent<SimpleInteraction>().enabled_ = false;
        cookies.GetComponent<SimpleInteraction>().InteractIcon.SetActive(false);
    }
}