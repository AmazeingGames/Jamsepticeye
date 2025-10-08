using JetBrains.Annotations;
using UnityEngine;

public static class ServiceLocator 
{
    static IDialogueService dialogueService;
    static IInventoryDataService inventoryDataService;
    static ICutscenesService cutscenesService;
    static ISceneHelperService sceneHelperService;
    static ITilemapHelperService tilemapHelperService;
    static IGameFlowService gameFlowService;

    public static void ProvideDialogueService(IDialogueService dialogueService)
        => ServiceLocator.dialogueService = dialogueService;

    public static IDialogueService GetDialogueService()
        => dialogueService;


    public static void ProvideInventoryService(IInventoryDataService inventoryService)
        => inventoryDataService = inventoryService;

    public static IInventoryDataService GetInventoryService()
        => inventoryDataService;


    public static void ProvideCutscenesService(ICutscenesService cutscenesService)
        => ServiceLocator.cutscenesService = cutscenesService;

    public static ICutscenesService GetCutscenesService()
        => cutscenesService;

    public static void ProvideSceneHelperService(ISceneHelperService sceneHelper)
        => sceneHelperService = sceneHelper;

    public static ISceneHelperService GetSceneHelperSerivce()
        => sceneHelperService;

    public static void ProvideTilemapHelperService(ITilemapHelperService tilemapHelper)
        => tilemapHelperService = tilemapHelper;

    public static ITilemapHelperService GetTilemapHelperSerivce()
        => tilemapHelperService;

    public static void ProvideGameFlowService(IGameFlowService gameFlow)
        => gameFlowService = gameFlow;

    public static IGameFlowService GetGameFlowSerivce()
        => gameFlowService;
}


