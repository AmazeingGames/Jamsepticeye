using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class InteractNestScript : MonoBehaviour
{
    [SerializeField] GameObject hammock;
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject rock;
    [SerializeField] GameObject nest;
    [SerializeField] TextAsset inkJSON;
    void Start()
    {

    }

    void Update()
    {
        var dialogueInteraction = GetComponentInParent<DialogueInteraction>();
        if (dialogueInteraction != null)
        {
            if (GameStateScript.instance.Is(GameState.PLACED_HAMMOCK))
            {
                if (hammock != null && !hammock.activeInHierarchy)
                {
                    nest.GetComponent<CircleCollider2D>().enabled = false; // Disable large collider 
                    nest.GetComponent<BoxCollider2D>().enabled = true; // Enable small collider
                    // We put the hammock down
                    StartCoroutine(PutHammock());
                    FadeController.instance.TriggerFade();
                }
                else if (GameStateScript.instance.Is(GameState.NEST_ROCKING_STARTS) && !GameStateScript.instance.Is(GameState.ROCK_THROWN))
                {
                    // We are ready to throw!
                    GameStateScript.instance.Set(GameState.ROCK_THROWN);

                    // Trigger animation
                    playerController.dynamicMover.MoveTo(new Vector2(-15.5f, -16.5f), new Vector2(-1, 0), () =>
                    {
                        rock.SetActive(true);
                        Vector3[] waypoints = new[] { new Vector3(-16.85424f, -14.49778f, 0f), new Vector3(-18.63988f, -13.98094f, 0f) };

                        rock.transform.DOPath(waypoints, 1f, PathType.CatmullRom)
                            .SetEase(Ease.InOutQuad).OnComplete(() =>
                            {
                                rock.SetActive(false);
                                Vector3[] waypoints = new[] { new Vector3(-18.75f, -14.62341f, 0f), new Vector3(-18.75f, -15.3031f, 0f), new Vector3(-18.75f, -15.90409f, 0f) };
                                //float[] rotations = new float[] { 120.0f, 240.0f, 360.0f };
                                // Add the path
                                nest.transform.DOPath(waypoints, 2f, PathType.CatmullRom).SetEase(Ease.InOutQuad).OnComplete(() =>
                                {
                                    DialogueManager.GetInstance().PlayDialogue(inkJSON);
                                });
                                //Sequence seq = DOTween.Sequence();
                                //seq.Join(pathTween);

                                // Add rotation tween that syncs with path
                                // float duration = 3f / (waypoints.Length - 1); // Duration per segment

                                //for (int i = 0; i < rotations.Length - 1; i++)
                                //{
                                //    seq.Insert(i * duration, transform.DORotate(
                                //        new Vector3(0, 0, rotations[i + 1]),
                                //        duration
                                //    ));
                                //}
                            });
                    });
                }
            }
            if (GameStateScript.instance.Is(GameState.NEEDS_EGGS))
            {
                dialogueInteraction.Enable();
            }
            else
            {
                dialogueInteraction.Disable();
            }
        }
    }

    private IEnumerator PutHammock()
    {
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine("SetupHammockScene");
    }

    private IEnumerator SetupHammockScene()
    {
        hammock.SetActive(true);
        playerController.transform.position = new Vector2(-17.4f, -16.7f);

        yield return null;
    }
}
