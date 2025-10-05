using UnityEngine;
using UnityEngine.EventSystems;

public class BakerScript : MonoBehaviour
{
    private Animator animator;
    [SerializeField]

    PathFollower pathFollower;
    DialogueInteraction dialogueInteraction;
    BoxCollider2D boxCollider;
    public void Start()
    {
        animator = GetComponent<Animator>();
        // Look to the left at the beginning (at the player)
        animator.SetFloat("LookX", -1);
        animator.SetFloat("LookY", 0);

        pathFollower = GetComponentInParent<PathFollower>();
        dialogueInteraction = GetComponentInParent<DialogueInteraction>();
        boxCollider = GetComponentInParent<BoxCollider2D>();

    }
    public void Update()
    {
        if (pathFollower != null)
        {
            if (!pathFollower.pathStarted)
            {
                // Hasn't started the path animation
                if (!GameStateScript.instance.Is(GameState.HAS_EGGS) && !GameStateScript.instance.Is(GameState.HAS_SUGAR))
                {
                    // Player got through the cutscene and gave his ingredients to the baker. The baker should start moving.
                    pathFollower.StartPath();
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
            }
        }
        if (boxCollider != null)
            boxCollider.enabled = true;
        dialogueInteraction.Enable();
    }
}
