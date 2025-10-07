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
        var tileUnderStepper = ServiceLocator.GetTilemapHelperSerivce().GetTileUnderObject(gameObject);

        Assert.IsNotNull(tileUnderStepper, "No tile exists below stepper.");

        DataTile dataTile = tileUnderStepper as DataTile;

        // Assert.IsNotNull(dataTile, "Tile under stepper is not a data tile");

        if (dataTile == null)
        {
            Debug.LogError("Tile under stepper is not a data tile: cannot trigger footstep.");
            return;
        }

        SteppedEventHandler?.Invoke(this, new SteppedEventArgs(gameObject, dataTile));
    }

}
