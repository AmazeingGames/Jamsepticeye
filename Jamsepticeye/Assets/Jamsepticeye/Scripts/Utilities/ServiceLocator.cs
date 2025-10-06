using UnityEngine;

public static class ServiceLocator 
{
    static IDialogueService dialogueService;
    static IInventoryDataService inventoryDataService;
    static ICutscenesService cutscenesService;

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
}


