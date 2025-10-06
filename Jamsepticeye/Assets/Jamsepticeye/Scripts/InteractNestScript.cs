using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class InteractNestScript : MonoBehaviour
{
    [SerializeField] GameObject hammockContainer;
    [SerializeField] GameObject hammockObject;
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject rock;
    [SerializeField] GameObject nest;
    [SerializeField] TextAsset inkJSON;
    void Start()
    {

    }

    void Update()
    {
        bool disabledExplicit = false;
        var dialogueInteraction = GetComponentInParent<DialogueInteraction>();
        if (dialogueInteraction != null)
        {
            if (GameStateScript.Instance.Is(GameState.PLACED_HAMMOCK))
            {
                if (hammockObject != null && !hammockObject.activeInHierarchy
                    && GameStateScript.Instance.Is(GameState.NEEDS_EGGS)
                    && !GameStateScript.Instance.Is(GameState.NEST_ROCKING_STARTS))
                {
                    //nest.GetComponent<CircleCollider2D>().enabled = false; // Disable large collider 
                    //nest.GetComponent<BoxCollider2D>().enabled = true; // Enable small collider
                    // We put the hammock down
                    StartCoroutine(PutHammock());
                    FadeController.instance.TriggerFade();
                }
                else if (GameStateScript.Instance.Is(GameState.NEST_ROCKING_STARTS) && !GameStateScript.Instance.Is(GameState.ROCK_THROWN))
                {
                    // We are ready to throw!
                    GameStateScript.Instance.Set(GameState.ROCK_THROWN);
                    hammockContainer.SetActive(true);
                    hammockObject.SetActive(false);
                    dialogueInteraction.Disable();
                    disabledExplicit = true;
                    // Trigger animation
                    playerController.dynamicMover.MoveTo(new Vector2(-15.5f, -16.5f), new Vector2(-1, 0), () =>
                    {

                        rock.SetActive(true);
                        Vector3[] waypoints = new[] { new Vector3(-16.85424f, -14.49778f, 0f), new Vector3(-18.63988f, -13.98094f, 0f) };

                        rock.transform.DOPath(waypoints, 1f, PathType.CatmullRom)
                            .SetEase(Ease.InOutQuad).OnComplete(() =>
                            {
                                rock.SetActive(false);
                                Vector3[] waypoints = new[] { new Vector3(-18.75f, -14.62341f, 0f), new Vector3(-18.75f, -15.3031f, 0f), new Vector3(-18.75f, -16.3f, 0f) };
                                nest.transform.DOPath(waypoints, 2f, PathType.CatmullRom).SetEase(Ease.InOutQuad).OnComplete(() =>
                                {
                                    DialogueManager.GetInstance().PlayDialogue(inkJSON);
                                    StartCoroutine(GrabHammockAndEggs());
                                });
                            });
                    });
                }
            }
            if (!disabledExplicit)
            {
                if (GameStateScript.Instance.Is(GameState.NEEDS_EGGS))
                {
                    dialogueInteraction.Enable();
                }
                else
                {
                    dialogueInteraction.Disable();
                }
            }
        }
    }

    private IEnumerator PutHammock()
    {
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine("SetupHammockScene");
    }

    private IEnumerator GrabHammockAndEggs()
    {
        while (DialogueManager.GetInstance().IsDialoguePlaying)
            yield return new WaitForSeconds(0.1f);
        FadeController.instance.TriggerFade();

        yield return new WaitForSeconds(1.5f);

        hammockObject.SetActive(false);
        hammockContainer.SetActive(false);
        nest.SetActive(false);
        ServiceLocator.GetInventoryService().CollectItem(ItemData.ItemType.Eggs);
        GameStateScript.Instance.Set(GameState.HAS_EGGS);
        GameStateScript.Instance.Unset(GameState.NEEDS_EGGS);
        GameStateScript.Instance.Unset(GameState.PLACED_HAMMOCK);
        playerController.transform.position = new Vector2(-17.8f, -16.84f);
        yield return null;
    }

    private IEnumerator SetupHammockScene()
    {
        hammockObject.SetActive(true);
        playerController.transform.position = new Vector2(-17.7f, -16.84f);

        yield return null;
    }

}
