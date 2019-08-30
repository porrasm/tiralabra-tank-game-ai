using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data structure to store which coordinates are used. Similar functionality than a HashMap.
/// </summary>
public class CoordsContainer {

    #region fields
    private bool[,] coords;

    private int count;
    #endregion


    public CoordsContainer() {
        coords = new bool[TankSettings.LevelWidth, TankSettings.LevelHeight];
    }

    /// <summary>
    /// Amount of coordinates added
    /// </summary>
    public int Count {
         get {
            return count;
        }
    }

    /// <summary>
    /// Adds a specific IntCoords value to the set
    /// </summary>
    /// <param name="newCoords"></param>
    public void Add(IntCoords newCoords) {
        if (!Contains(newCoords)) {
            count++;
            coords[newCoords.x, newCoords.y] = true;
        }
    }

    /// <summary>
    /// Checks if the specific coords have been added
    /// </summary>
    /// <param name="newCoords"></param>
    /// <returns></returns>
    public bool Contains(IntCoords newCoords) {
        return coords[newCoords.x, newCoords.y];
    }
}
