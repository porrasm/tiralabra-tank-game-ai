using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPath {

    #region fields
    public IntCoords Coords;

    public TankPath Next { get; set; }
    public TankPath Prev { get; set; }
    #endregion

    public TankPath(int x, int y) {
        Prev = null;
        Coords = new IntCoords(x, y);
        Next = null;
    }
    public TankPath(TankPath prev, int x, int y) {
        Prev = prev;
        Coords = new IntCoords(x, y);
        Next = null;
    }

   
}
