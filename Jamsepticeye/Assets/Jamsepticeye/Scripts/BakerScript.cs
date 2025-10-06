using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BakerScript : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    PathFollower pathFollower;
    DialogueInteraction dialogueInteraction;
    BoxCollider2D boxCollider;
    [SerializeField]
    PolygonCollider2D collider_;
    [SerializeField]
    GameObject cookies;

    [SerializeField]
    List<GameObject> itemsDisappearOnDeath = new List<GameObject>();
    [SerializeField]
    List<GameObject> itemsAppearOnDeath = new List<GameObject>();

    [SerializeField] CutsceneSequence bakerMagic;
    [SerializeField]
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

        if (!DialogueManager.GetInstance().IsDialoguePlaying && GameStateScript.Instance.Is(GameState.FLOUR_MAGIC_READY))
        {
            // Magic time!!

            // TODO: Cutscene baker death

            ServiceLocator.GetCutscenesService().TriggerCutsceneSequence(bakerMagic);
            GameStateScript.Instance.Unset(GameState.FLOUR_MAGIC_READY);

            foreach (var item in itemsAppearOnDeath)
                item.SetActive(true);

            foreach (var item in itemsDisappearOnDeath)
                item.SetActive(false);

            return;
        }
        if (pathFollower != null)
        {
            if (!pathFollower.pathStarted)
            {
                // Hasn't started the path animation
                if (!GameStateScript.Instance.Is(GameState.HAS_EGGS) && !GameStateScript.Instance.Is(GameState.HAS_SUGAR))
                {
                    // Player got through the cutscene and gave his ingredients to the baker. The baker should start moving.
                    pathFollower.StartPath();
                    collider_.enabled = false; // No interaction with the trigger zone
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
                else if (pathFollower.pathComplete && !GameStateScript.Instance.Is(GameState.COOKIES_BAKED))
                {
                    // He just arrives at his spot
                    GameStateScript.Instance.Set(GameState.COOKIES_BAKED);
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
        yield return new WaitForSeconds(1f);
        cookies.SetActive(true);
    }
}