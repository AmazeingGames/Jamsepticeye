using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class YoungBoyScript : MonoBehaviour
{
    Animator animator;
    DialogueInteraction dialogueInteraction;
    [SerializeField] TextAsset chokingDialog;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        dialogueInteraction = GetComponentInParent<DialogueInteraction>();
        if (animator != null)
        {
            if (GameStateScript.Instance.Is(GameState.KID_CHOKING))
            {
                transform.position = new Vector2(transform.position.x - 0.7f, transform.position.y);
                animator.SetBool("AteCookies", true);
            }
        }
    }

    void Update()
    {
        if (!DialogueManager.GetInstance().IsDialoguePlaying && GameStateScript.Instance.Is(GameState.KID_CHOKING_DIALOG))
        {
            GameStateScript.Instance.Unset(GameState.KID_CHOKING_DIALOG);
            ServiceLocator.GetDialogueService().PlayDialogue(chokingDialog);
        }

        if (animator != null)
        {
            bool isKidTalking = DialogueManager.GetInstance().IsDialoguePlaying && DialogueManager.GetInstance().currentSpeaker.Name == DialogueManager.GetInstance().kid.Name;
            animator.SetBool("IsTalking", isKidTalking);
        }
        var dialogueInteraction = GetComponentInParent<DialogueInteraction>();
        if (dialogueInteraction != null)
        {
            if (!DialogueManager.GetInstance().IsDialoguePlaying && GameStateScript.Instance.Is(GameState.KID_FED) && !GameStateScript.Instance.Is(GameState.KID_CHOKING))
            {
                // Only start the coroutine once thanks to this condition
                GameStateScript.Instance.Set(GameState.KID_CHOKING);
                StartCoroutine(TeleportKid());
            }
        }
    }
    private IEnumerator TeleportKid()
    {
        FadeController.instance.TriggerFade();

        yield return new WaitForSeconds(1f);
        transform.position = new Vector2(transform.position.x - 0.7f, transform.position.y);
        yield return new WaitForSeconds(1f);
        animator.SetBool("AteCookies", true);
        yield return new WaitForSeconds(2f);

        GameStateScript.Instance.Set(GameState.KID_CHOKING_DIALOG);
        yield return null;
    }
}
