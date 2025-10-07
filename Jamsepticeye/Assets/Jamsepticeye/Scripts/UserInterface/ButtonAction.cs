using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ButtonAction", menuName = "ScriptableObjects/ButtonAction")]
public class ButtonAction : ScriptableObject
{
    public enum ActionType { None, UI, Game }
    public enum UIAction { None, OpenMenu } // Note: this would be contained in a 'UIManager' esque class
    public enum MenuToOpen { None, Main } // Note: this would be contained in a 'UIManager' esque class
    public enum GameAction { None, PlayGame, QuitGame } // Note: this would be contained in a 'GameManager' esque class

    [SerializeField] ActionType myActionType;

    [HideIfGroup("ShouldHideGameAction")]
    [SerializeField] GameAction myGameAction;

    [HideIfGroup("ShouldHideUIAction")]
    [SerializeField] UIAction myUIAction;

    [HideIf("ShouldHideMenuToOpen")]
    [SerializeField] MenuToOpen menuToOpen;


    bool ShouldHideGameAction => myActionType != ActionType.Game;
    bool ShouldHideUIAction => myActionType != ActionType.UI;
    bool ShouldHideMenuToOpen => myUIAction != UIAction.OpenMenu || ShouldHideUIAction;
}
