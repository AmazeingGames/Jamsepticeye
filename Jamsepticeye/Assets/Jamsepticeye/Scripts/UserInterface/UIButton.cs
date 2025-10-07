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
    public enum Interaction { None, Enter, Exit, Click, }


    public static EventHandler<InteractingEventArgs> InteractingEventHandler;

    public class InteractingEventArgs : EventArgs 
    {
        public readonly Interaction myInteraciton;
        public InteractingEventArgs(Interaction myInteraciton)
        {
            this.myInteraciton = myInteraciton;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnInteracting(Interaction.Click);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnInteracting(Interaction.Enter);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnInteracting(Interaction.Exit);
    }

    void OnInteracting(Interaction myInteraction) 
    { 
        InteractingEventHandler?.Invoke(this, new InteractingEventArgs(myInteraction)); 
    }

}
