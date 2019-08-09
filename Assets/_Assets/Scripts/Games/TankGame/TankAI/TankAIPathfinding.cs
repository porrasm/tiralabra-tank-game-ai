using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIPathfinding {

    protected byte[,] level;

    public TankAIPathfinding(byte[,] level) {
        this.level = level;
    }

    public delegate bool FoundCondition(IntCoords current);

    public Vector[] FindPath(IntCoords start, IntCoords end) {

        if (end.x >= level.Length || end.y >= level.GetLength(1)) {
            return new Vector[] { Vector.CoordsToPosition(start) };
        }

        bool FoundCondition(IntCoords current) {
            return current == end;
        }

        return FindPath(start, end, FoundCondition);
    }
    public virtual Vector[] FindPath(IntCoords start, IntCoords end, FoundCondition foundCondition) {
        return null;
    }
}
