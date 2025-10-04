using Unity.VisualScripting;
using UnityEngine;

public static class ServiceLocator 
{
    static IDialogueService dialogueService;

    public static void ProvideService(IDialogueService service)
        => dialogueService = service;

    public static IDialogueService GetDialogueService()
        => dialogueService;}
