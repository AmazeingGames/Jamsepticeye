using System;
using UnityEngine;
using static GameStateTracker;

public class GameStateTracker : MonoBehaviour
{
    [Flags] public enum NewGameState
    {
        None                        = 0,
        HasPlacedHammock            = 1 << 0,         
        DoesNeedSugar               = 1 << 1,
        DoesKnowBaker               = 1 << 2,
        HasTalkedToBaker            = 1 << 3,
        DoesNeedEggs                = 1 << 4,
        HasGivenBakerIngredients    = 1 << 5,
        HasMurderedBaker            = 1 << 6,
        HasFoundNest                = 1 << 7,
        HasFedKid                   = 1 << 8,
        HasNestStartedRocking       = 1 << 9,
        CanEnterBakery              = 1 << 10,
        CanPerformBakerMagic           = 1 << 11,
        HasThrownRock               = 1 << 12,
        IsKidChoking                = 1 << 13,
        HasBakedCookies             = 1 << 14,
        HasChokingDialogueStarted   = 1 << 15,
        HasHammockFadeStarted       = 1 << 16,
        HasStartedSceneEndingSetup  = 1 << 17,
        HasFinishedSceneEndingSetup = 1 << 18,
        HasPeepPoofed               = 1 << 19,
        DoesNeedSticks              = 1 << 20,
        DoesNeedRocks               = 1 << 21,
    };

    public static NewGameState myGameState;

    private void OnEnable()
    {
        DialogueManager.ChangedStateEventHandler                        += Dialogue_ChangedState;
        InventoryDataManager.ItemsInInventory.AddedItemEventHandler     += Inventory_AddItem;
        InventoryDataManager.ItemsInInventory.RemovedItemEventHandler   += Inventory_RemoveItem;
        CutscenesPlayer.ChangedStateEventHandler                        += Cutscenes_ChangedState;
        InteractNestScript.UpdatingNestCinematicEventHandler            += Cinematics_UpdatingNestCinematic;
        SceneRoot.EnablingRootEventHandler                              += Scenes_EnablingRoot;
        InteractNestScript.BuildingHammockEventHandler                  += Cinematics_BuildingHammock;
        BakerScript.PerformingActionEventHandler                        += NPC_Baker_PerformingAction;
    }

    private void OnDisable()
    {
        DialogueManager.ChangedStateEventHandler                        -= Dialogue_ChangedState;
        InventoryDataManager.ItemsInInventory.AddedItemEventHandler     -= Inventory_AddItem;
        InventoryDataManager.ItemsInInventory.RemovedItemEventHandler   -= Inventory_RemoveItem;
        CutscenesPlayer.ChangedStateEventHandler                        -= Cutscenes_ChangedState;
        InteractNestScript.UpdatingNestCinematicEventHandler            -= Cinematics_UpdatingNestCinematic;
        InteractNestScript.BuildingHammockEventHandler                  -= Cinematics_BuildingHammock;
        SceneRoot.EnablingRootEventHandler                              -= Scenes_EnablingRoot;
        BakerScript.PerformingActionEventHandler                        -= NPC_Baker_PerformingAction;
    }

    private void Start()
    {
        Set(NewGameState.DoesNeedRocks);
        Set(NewGameState.DoesNeedSticks);
    }

    private void NPC_Baker_PerformingAction(object sender, BakerScript.PerformingActionEventArgs e)
    {
        switch (e.myAction)
        {
            case BakerScript.Action.None:
                break;

            case BakerScript.Action.BakingCookies:
                Set(NewGameState.HasBakedCookies);
                break;
        }
    }

    private void Cinematics_BuildingHammock(object sender, InteractNestScript.BuildingHammockEventArgs e)
        => Set(NewGameState.HasPlacedHammock);

    private void Scenes_EnablingRoot(object sender, SceneRoot.EnablingRootEventArgs e)
    {
        switch (e.rootData.MyScene)
        {
            case SceneRootData.SceneType.None:
                break;

            case SceneRootData.SceneType.Village:
                break;

            case SceneRootData.SceneType.Bakery:
                break;

            case SceneRootData.SceneType.GroceryStore:
                break;

            case SceneRootData.SceneType.Menu:
                break;

            case SceneRootData.SceneType.Bootstrap:
                break;

            case SceneRootData.SceneType.Credits:
                break;
        }
    }

    private void Cinematics_UpdatingNestCinematic(object sender, InteractNestScript.UpdatingNestCinematicEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void Cutscenes_ChangedState(object sender, CutscenesPlayer.StateChangedEventArgs e)
    {
        switch (e.cutsceneSequence.MyCutscene)
        {
            case CutsceneSequence.Cutscene.None:
                break;

            case CutsceneSequence.Cutscene.BakerMagic:
                Unset(NewGameState.CanPerformBakerMagic);
                Set(NewGameState.HasMurderedBaker);
                break;

            case CutsceneSequence.Cutscene.OpeningSequence:
                break;
        }
    }

    private void Inventory_AddItem(object sender, ItemData e)
    {
        throw new NotImplementedException();
    }

    private void Inventory_RemoveItem(object sender, ItemData e)
    {
        throw new NotImplementedException();
    }

    private void Dialogue_ChangedState(object sender, DialogueManager.ChangedStateEventArgs e)
    {
        switch (e.myDialogueState)
        {
            case DialogueManager.DialogueState.None:
                break;

            case DialogueManager.DialogueState.Triggered:
                switch (e.speaker.MySpeaker)
                {
                    case Speaker.Character.None:
                        break;

                    case Speaker.Character.Baker:
                        Set(NewGameState.HasTalkedToBaker);
                        Set(NewGameState.DoesNeedEggs);
                        Set(NewGameState.DoesNeedSugar);
                        break;

                    case Speaker.Character.Peeper:
                        break;

                    case Speaker.Character.Tim:
                        break;

                    case Speaker.Character.HungryBoy:
                        Set(NewGameState.DoesKnowBaker);
                        break;

                    case Speaker.Character.DocDoor:
                        break;
                }

                break;

            case DialogueManager.DialogueState.Exited:
                break;

            case DialogueManager.DialogueState.Continued:
                break;
        }
    }

    public static bool IsGameState(NewGameState state)
        => (state & myGameState) == state;

    void Set(NewGameState state)
        => myGameState |= state;

    void Unset(NewGameState state)
        => myGameState &= ~state;

}
