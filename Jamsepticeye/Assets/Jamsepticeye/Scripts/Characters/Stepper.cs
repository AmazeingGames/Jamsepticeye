using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class Stepper : MonoBehaviour
{
    public static EventHandler<SteppedEventArgs> SteppedEventHandler;

    public class SteppedEventArgs : EventArgs 
    {
        public readonly GameObject gameObject;
        public readonly DataTile dataTile;
        public SteppedEventArgs(GameObject gameObject, DataTile tileUnderneath) 
        {
            this.gameObject = gameObject;
            this.dataTile = tileUnderneath;
        } 
    }

    public void OnStepped() 
    {
        DataTile dataTileUnderStepper = ServiceLocator.GetTilemapHelperSerivce().GetTileUnderObject(gameObject);

        if (dataTileUnderStepper == null)
        {
            Debug.LogWarning($"No tile exists below stepper {gameObject.name}");

            return;
        }
        
        
        SteppedEventHandler?.Invoke(this, new SteppedEventArgs(gameObject, dataTileUnderStepper));
    }

}
