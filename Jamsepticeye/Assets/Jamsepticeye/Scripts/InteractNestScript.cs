using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InteractNestScript : MonoBehaviour
{
    public enum CinematicPoint { None, Beginning, ThrowRock, NestFall, End }

    [SerializeField] GameObject hammockContainer;
    [SerializeField] GameObject hammockObject;
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject rock_left;
    [SerializeField] GameObject rock_right;
    [SerializeField] GameObject nest;
    [SerializeField] TextAsset inkJSON;

    public static EventHandler<UpdatingCinematicEventArgs> UpdatingCinematicEventHandler;

    public static EventHandler<BuildingHammockEventArgs> BuildingHammockEventHandler;

    public class BuildingHammockEventArgs : EventArgs { public BuildingHammockEventArgs() { } }

    void OnBuildingHammock() { BuildingHammockEventHandler?.Invoke(this, new BuildingHammockEventArgs()); }


    public class UpdatingCinematicEventArgs : EventArgs 
    {
        public readonly CinematicPoint myCinematicPoint;
        public UpdatingCinematicEventArgs(CinematicPoint myCinematicPoint) 
        {
            this.myCinematicPoint = myCinematicPoint;
        } 
    }

    readonly List<CinematicPoint> cinematicPointsReached = new();

    void OnUpdatingCinematic(CinematicPoint myCinematicPoint) 
    {
        Debug.Log("called");
        if (cinematicPointsReached.Contains(myCinematicPoint))
            return;

        Debug.Log("ran and invoke");
        cinematicPointsReached.Add(myCinematicPoint);
        UpdatingCinematicEventHandler?.Invoke(this, new UpdatingCinematicEventArgs(myCinematicPoint));

        if (myCinematicPoint == CinematicPoint.Beginning)
            cinematicPointsReached.Clear();
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
                    && !GameStateScript.Instance.Is(GameState.NEST_ROCKING_STARTS)
                    && !GameStateScript.Instance.Is(GameState.HAMMOCK_FADE_STARTED))
                {
                    // We put the hammock down
                    GameStateScript.Instance.Set(GameState.HAMMOCK_FADE_STARTED);
                    StartCoroutine(PutHammock());
                }
                else if (GameStateScript.Instance.Is(GameState.NEST_ROCKING_STARTS) && !GameStateScript.Instance.Is(GameState.ROCK_THROWN))
                {
                    OnUpdatingCinematic(CinematicPoint.Beginning);

                    // Staging throw!
                    GameStateScript.Instance.Set(GameState.ROCK_THROWN);
                    hammockContainer.SetActive(true);
                    hammockObject.SetActive(false);
                    dialogueInteraction.Disable();
                    disabledExplicit = true;

                    // Trigger animation

                    bool isPlayerOnLeftOfNest = nest.transform.position.x > playerController.gameObject.transform.position.x;
                    var destination = isPlayerOnLeftOfNest ? new Vector2(-20.88122f, -16.54031f) : new Vector2(-15.5f, -16.5f);
                    var lookDir = isPlayerOnLeftOfNest ? new Vector2(1, 0) : new Vector2(-1, 0);
                    var rock = isPlayerOnLeftOfNest ? rock_left : rock_right;

                    playerController.dynamicMover.MoveTo(destination, lookDir, () =>
                    {
                        rock.SetActive(true);
                        Vector3[] waypoints;
                        if (isPlayerOnLeftOfNest)
                            waypoints = new[] { new Vector3(-20.51924f, -15.20031f, 0f), new Vector3(-19.7347f, -14.54231f, 0f), new Vector3(-18.77301f, -14.08677f, 0f) };
                        else
                            waypoints = new[] { new Vector3(-16.85424f, -14.49778f, 0f), new Vector3(-18.63988f, -13.98094f, 0f) };

                        OnUpdatingCinematic(CinematicPoint.ThrowRock);

                        rock.transform.DOPath(waypoints, 1f, PathType.CatmullRom)
                            .SetEase(Ease.InOutQuad).OnComplete(() =>
                            {
                                rock.SetActive(false);
                                Vector3[] waypoints = new[] { new Vector3(-18.74999f, -16.355f, 0f), new Vector3(-18.75004f, -16.15963f, 0f), new Vector3(-18.7532f, -16.35219f, 0f), new Vector3(-18.75004f, -16.27959f, 0f), new Vector3(-18.75004f, -16.35219f, 0f) };

                                OnUpdatingCinematic(CinematicPoint.NestFall);

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
        OnBuildingHammock();

        FadeController.instance.TriggerFade();
        yield return new WaitForSeconds(1.5f);
        hammockObject.SetActive(true);
        playerController.transform.position = new Vector2(-17.7f, -16.84f);

        yield return null;
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
}
