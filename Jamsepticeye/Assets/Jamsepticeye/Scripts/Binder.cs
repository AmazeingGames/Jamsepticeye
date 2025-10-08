using Ink.Runtime;
using UnityEngine;

public class Binder
{
    public static void Bind(Story story)
    {
        story.variablesState["KNOWS_ABOUT_BAKER"] = GameStateScript.Instance.Is(GameState.KNOWS_ABOUT_BAKER);
        story.variablesState["TALKED_TO_BAKER"] = GameStateScript.Instance.Is(GameState.TALKED_TO_BAKER);
        story.variablesState["HAS_COOKIES"] = GameStateScript.Instance.Is(GameState.HAS_COOKIES);
        story.variablesState["HAS_SUGAR"] = GameStateScript.Instance.Is(GameState.HAS_SUGAR);
        story.variablesState["HAS_COFFEE"] = GameStateScript.Instance.Is(GameState.HAS_COFFEE);
        story.variablesState["HAS_EGGS"] = GameStateScript.Instance.Is(GameState.HAS_EGGS);
        story.variablesState["HAS_ROCKS"] = GameStateScript.Instance.Is(GameState.HAS_ROCKS);
        story.variablesState["HAS_STICKS"] = GameStateScript.Instance.Is(GameState.HAS_STICKS);
        story.variablesState["NEEDS_SUGAR"] = GameStateScript.Instance.Is(GameState.NEEDS_SUGAR);
        story.variablesState["NEEDS_EGGS"] = GameStateScript.Instance.Is(GameState.NEEDS_EGGS);
        story.variablesState["NEEDS_STICKS"] = GameStateScript.Instance.Is(GameState.NEEDS_STICKS);
        story.variablesState["FOUND_NEST"] = GameStateScript.Instance.Is(GameState.FOUND_NEST);
        story.variablesState["NEEDS_ROCKS"] = GameStateScript.Instance.Is(GameState.NEEDS_ROCKS);
        story.variablesState["NEST_ROCKING_STARTS"] = GameStateScript.Instance.Is(GameState.NEST_ROCKING_STARTS);
        story.variablesState["BAKER_DEAD"] = GameStateScript.Instance.Is(GameState.BAKER_DEAD);
        story.variablesState["PLACED_HAMMOCK"] = GameStateScript.Instance.Is(GameState.PLACED_HAMMOCK);
        story.variablesState["ALLOWED_BAKERY"] = GameStateScript.Instance.Is(GameState.ALLOWED_BAKERY);
        story.variablesState["FLOUR_MAGIC_READY"] = GameStateScript.Instance.Is(GameState.FLOUR_MAGIC_READY);
        story.variablesState["END_SCENE_SETUP"] = GameStateScript.Instance.Is(GameState.END_SCENE_SETUP);

        story.BindExternalFunction("SetKnowsAboutBaker", () => GameStateScript.Instance.Set(GameState.KNOWS_ABOUT_BAKER));
        story.BindExternalFunction("SetHasCoffee", () =>
        {
            GameStateScript.Instance.Set(GameState.HAS_COFFEE);
            ServiceLocator.GetInventoryService().CollectItem(ItemData.ItemType.Coffee);
        });
        story.BindExternalFunction("SetKidFed", () =>
        {
            GameStateScript.Instance.Set(GameState.KID_FED);
            GameStateScript.Instance.Unset(GameState.HAS_COOKIES);
            ServiceLocator.GetInventoryService().UseItem(ItemData.ItemType.Cookies);
        });
        story.BindExternalFunction("SetHasSugar", () =>
        {
            ServiceLocator.GetInventoryService().CollectItem(ItemData.ItemType.Sugar);
            GameStateScript.Instance.Set(GameState.HAS_SUGAR);
            GameStateScript.Instance.Unset(GameState.NEEDS_SUGAR);
        });
        story.BindExternalFunction("SetHasRocks", () =>
        {
            ServiceLocator.GetInventoryService().CollectItem(ItemData.ItemType.Rocks);
            GameStateScript.Instance.Set(GameState.HAS_ROCKS);
            GameStateScript.Instance.Unset(GameState.NEEDS_ROCKS);
        });
        story.BindExternalFunction("SetHasSticks", () =>
        {
            ServiceLocator.GetInventoryService().CollectItem(ItemData.ItemType.Stick);
            GameStateScript.Instance.Set(GameState.HAS_STICKS);
            GameStateScript.Instance.Unset(GameState.NEEDS_STICKS);
        });
        story.BindExternalFunction("SetHammockPlaced", () =>
        {
            GameStateScript.Instance.Set(GameState.PLACED_HAMMOCK);
            GameStateScript.Instance.Unset(GameState.HAS_STICKS);
            ServiceLocator.GetInventoryService().UseItem(ItemData.ItemType.Stick);
        });
        story.BindExternalFunction("SetNestRocked", () =>
        {
            GameStateScript.Instance.Set(GameState.NEST_ROCKING_STARTS);
            GameStateScript.Instance.Unset(GameState.HAS_ROCKS);
            ServiceLocator.GetInventoryService().UseItem(ItemData.ItemType.Rocks);
        });
        story.BindExternalFunction("SetTalkedToBaker", () =>
        {
            GameStateScript.Instance.Set(GameState.TALKED_TO_BAKER);
            GameStateScript.Instance.Set(GameState.NEEDS_SUGAR);
            GameStateScript.Instance.Set(GameState.NEEDS_EGGS);
        });
        story.BindExternalFunction("SetAllowBakery", () =>
        {
            GameStateScript.Instance.Set(GameState.ALLOWED_BAKERY);
        });
        story.BindExternalFunction("GiveIngredientsToBaker", () =>
        {
            Debug.Log("Gave ingredients to baker");
            GameStateScript.Instance.Unset(GameState.HAS_SUGAR);
            GameStateScript.Instance.Unset(GameState.HAS_EGGS);
            ServiceLocator.GetInventoryService().UseItem(ItemData.ItemType.Sugar);
            ServiceLocator.GetInventoryService().UseItem(ItemData.ItemType.Eggs);
        });
        story.BindExternalFunction("PrepareFlourMagicTrick", () =>
        {
            GameStateScript.Instance.Set(GameState.FLOUR_MAGIC_READY);
        });
        story.BindExternalFunction("SetBakerDead", () =>
        {
            GameStateScript.Instance.Set(GameState.BAKER_DEAD);
            GameStateScript.Instance.Unset(GameState.FLOUR_MAGIC_READY);
        });
        story.BindExternalFunction("EndGame", () =>
        {
            FadeController.instance.TriggerFadeForever();
            ServiceLocator.GetGameFlowSerivce().EndGame();
        });
        story.BindExternalFunction("SetFoundNest", () =>
        {
            GameStateScript.Instance.Set(GameState.FOUND_NEST);
        });
        story.BindExternalFunction("SetupEndScene", () =>
        {
            GameStateScript.Instance.Set(GameState.END_SCENE_SETUP);
        });
        story.BindExternalFunction("PeepGoesPoof", () =>
        {
            GameStateScript.Instance.Set(GameState.PEEP_POOFED);
        });
    }
}
