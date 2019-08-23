using System.Collections;
using System.Collections.Generic;

public class TankAIPathfinding {

    protected byte[,] level;

    public int ProcessedCount { get; protected set; }

    public TankAIPathfinding(byte[,] level) {
        this.level = level;
    }

    public delegate bool FoundCondition(IntCoords current);

    /// <summary>
    /// Finds a path from start coordinates to the end coordinates.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns>Path as Vector array</returns>
    public Vector[] FindPath(IntCoords start, IntCoords end) {

        if (end.x >= level.Length || end.y >= level.GetLength(1)) {
            return new Vector[] { Vector.CoordsToPosition(start) };
        }

        bool FoundCondition(IntCoords current) {
            return current == end;
        }

        return FindPath(start, end, FoundCondition);
    }

    /// <summary>
    /// Finds a path from start towards the end with an independent route found condition. The FoundCondition(IntCoords current) function is called on every cell and if it returns true the current route is returned.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="foundCondition"></param>
    /// <returns>Path as Vector array</returns>
    public virtual Vector[] FindPath(IntCoords start, IntCoords end, FoundCondition foundCondition) {
        ProcessedCount = 0;
        return null;
    }
}
