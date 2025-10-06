using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// One of the few class responsible for conveying important changes in game state directly to the game manager
public class UIButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum ButtonFunctionality { None, StartGame, OpenCredits, EnterMainMenu }

    public static EventHandler<ButtonInteractEventArgs> ButtonInteractEventHandler;

    [SerializeField] ButtonFunctionality myButtonFunctionality;

    public class ButtonInteractEventArgs : EventArgs
    {
        public enum InteractType { PointerEnter, PointerExit, PointerClick }
        public readonly InteractType myInteractType;

        public ButtonInteractEventArgs(InteractType myInteractType)
        {
            this.myInteractType = myInteractType;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        ButtonInteractEventHandler?.Invoke(this, new(ButtonInteractEventArgs.InteractType.PointerClick));

        switch (myButtonFunctionality)
        {
            case ButtonFunctionality.StartGame:
                break;
            case ButtonFunctionality.OpenCredits:
                break;
            case ButtonFunctionality.EnterMainMenu:
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonInteractEventHandler?.Invoke(this, new(ButtonInteractEventArgs.InteractType.PointerEnter));

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonInteractEventHandler?.Invoke(this, new(ButtonInteractEventArgs.InteractType.PointerExit));
    }
}
