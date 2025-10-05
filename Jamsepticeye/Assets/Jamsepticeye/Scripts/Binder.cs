using Ink.Runtime;
using UnityEngine;

public class Binder
{
    public static void Bind(Story story)
    {
        story.variablesState["KNOWS_ABOUT_BAKER"] = GameStateScript.instance.Is(GameState.KNOWS_ABOUT_BAKER);
        story.variablesState["TALKED_TO_BAKER"] = GameStateScript.instance.Is(GameState.TALKED_TO_BAKER);
        story.variablesState["HAS_COOKIES"] = GameStateScript.instance.Is(GameState.HAS_COOKIES);
        story.variablesState["HAS_SUGAR"] = GameStateScript.instance.Is(GameState.HAS_SUGAR);
        story.variablesState["HAS_COFFEE"] = GameStateScript.instance.Is(GameState.HAS_COFFEE);
        story.variablesState["HAS_EGGS"] = GameStateScript.instance.Is(GameState.HAS_EGGS);
        story.variablesState["HAS_ROCKS"] = GameStateScript.instance.Is(GameState.HAS_ROCKS);
        story.variablesState["HAS_STICKS"] = GameStateScript.instance.Is(GameState.HAS_STICKS);
        story.variablesState["NEEDS_SUGAR"] = GameStateScript.instance.Is(GameState.NEEDS_SUGAR);
        story.variablesState["NEEDS_EGGS"] = GameStateScript.instance.Is(GameState.NEEDS_EGGS);
        story.variablesState["NEEDS_STICKS"] = GameStateScript.instance.Is(GameState.NEEDS_STICKS);
        story.variablesState["FOUND_NEST"] = GameStateScript.instance.Is(GameState.FOUND_NEST);
        story.variablesState["NEEDS_ROCKS"] = GameStateScript.instance.Is(GameState.NEEDS_ROCKS);
        story.variablesState["NEST_ROCKED"] = GameStateScript.instance.Is(GameState.NEST_ROCKED);
        story.variablesState["BAKER_DEAD"] = GameStateScript.instance.Is(GameState.BAKER_DEAD);
        story.variablesState["PLACED_HAMMOCK"] = GameStateScript.instance.Is(GameState.PLACED_HAMMOCK);
        story.variablesState["ALLOWED_BAKERY"] = GameStateScript.instance.Is(GameState.ALLOWED_BAKERY);
        story.variablesState["FLOUR_MAGIC_READY"] = GameStateScript.instance.Is(GameState.FLOUR_MAGIC_READY);

        story.BindExternalFunction("SetKnowsAboutBaker", () => GameStateScript.instance.Set(GameState.KNOWS_ABOUT_BAKER));
        story.BindExternalFunction("SetHasCoffee", () => GameStateScript.instance.Set(GameState.HAS_COFFEE));
        story.BindExternalFunction("SetKidFed", () => {
            GameStateScript.instance.Set(GameState.KID_FED);
            GameStateScript.instance.Unset(GameState.HAS_COOKIES);
        });
        story.BindExternalFunction("SetHasSugar", () => {
            GameStateScript.instance.Set(GameState.HAS_SUGAR);
            GameStateScript.instance.Unset(GameState.NEEDS_SUGAR);
        });
        story.BindExternalFunction("SetHasRocks", () => {
            GameStateScript.instance.Set(GameState.HAS_ROCKS);
            GameStateScript.instance.Unset(GameState.NEEDS_ROCKS);
        });
        story.BindExternalFunction("SetHasSticks", () => {
            GameStateScript.instance.Set(GameState.HAS_STICKS);
            GameStateScript.instance.Unset(GameState.NEEDS_STICKS);
        });
        story.BindExternalFunction("SetHammockPlaced", () => {
            GameStateScript.instance.Set(GameState.PLACED_HAMMOCK);
            GameStateScript.instance.Unset(GameState.HAS_STICKS);
        });
        story.BindExternalFunction("SetNestRocked", () => {
            GameStateScript.instance.Set(GameState.NEST_ROCKED);
            GameStateScript.instance.Unset(GameState.HAS_ROCKS);
          //  GameStateScript.instance.Unset(GameState.NEEDS_EGGS);
        });
        story.BindExternalFunction("SetTalkedToBaker", () => {
            GameStateScript.instance.Set(GameState.TALKED_TO_BAKER);
            GameStateScript.instance.Set(GameState.NEEDS_SUGAR);
            GameStateScript.instance.Set(GameState.NEEDS_EGGS);
        });
        story.BindExternalFunction("SetAllowBakery", () => {
            GameStateScript.instance.Set(GameState.ALLOWED_BAKERY);
        });
        story.BindExternalFunction("GiveIngredientsToBaker", () => {
            GameStateScript.instance.Unset(GameState.HAS_SUGAR);
            GameStateScript.instance.Unset(GameState.HAS_EGGS);
        });
        story.BindExternalFunction("PrepareFlourMagicTrick", () => {
            GameStateScript.instance.Set(GameState.FLOUR_MAGIC_READY);
        });
    }
}
