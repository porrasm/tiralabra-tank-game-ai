using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CoverInReport]
public class TankDFSPath : TankAIPathfinding {

    #region fields
    private byte[,] visited;
    private byte[,] usedDirections;

    private IntCoords start, end;

    private IntCoords coords;

    private bool building;
    private CStack<IntCoords> route;

    private bool found;

    private FoundCondition foundCondition;

    public bool Building { get; private set; }
    #endregion

    public TankDFSPath(byte[,] level) : base(level) {
    }

    /// <summary>
    /// Finds a path from start towards the end with an independent route found condition. The FoundCondition(IntCoords current) function is called on every cell and if it returns true the current route is returned.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="foundCondition"></param>
    /// <returns>Path as Vector array</returns>
    public override Vector[] FindPath(IntCoords start, IntCoords end, FoundCondition foundCondition) {

        base.FindPath(start, end, foundCondition);

        this.start = start;
        this.end = end;

        this.foundCondition = foundCondition;


        visited = new byte[level.GetLength(0), level.GetLength(1)];
        usedDirections = new byte[level.GetLength(0), level.GetLength(1)];
        route = new CStack<IntCoords>();

        found = false;

        DFSRecursiveSearch(start);

        Vector[] vRoute = new Vector[route.Count];
        for (int i = vRoute.Length - 1; i > -1; i--) {
            vRoute[i] = Vector.CoordsToPosition(route.Pop());
        }

        return vRoute;
    }

    /// <summary>
    /// Recursively find route from start to end using DFS
    /// </summary>
    /// <param name="coords"></param>
    /// <param name="direction"></param>
    private void DFSRecursiveSearch(IntCoords coords) {

        ProcessedCount++;

        if (found) {
            return;
        }

        route.Push(coords);

        if (foundCondition(coords)) {
            found = true;
            return;
        }

        Visit(coords, 1);

        for (int i = 0; i < 8; i++) {

            TankDirection d = BestDirection(coords);
            if (d == TankDirection.None) {
                break;
            }

            DFSRecursiveSearch(coords.MoveToDirection(d));
        }

        if (!found) {
            route.Pop();
        }
    }

    /// <summary>
    /// Returns the direction which leads to the closest linked cell to the goal node.
    /// </summary>
    /// <param name="coords"></param>
    /// <returns></returns>
    private TankDirection BestDirection(IntCoords coords) {

        byte allowed = level[coords.x, coords.y];
        byte used = usedDirections[coords.x, coords.y];

        TankDirection best = TankDirection.None;
        float bestDistance = 0;

        for (int i = 0; i < 8; i++) {
            TankDirection direction = (TankDirection)i;

            if (!TankDirectionTools.AllowedDirection(allowed, direction) ||
                TankDirectionTools.AllowedDirection(used, direction) ||
                Visited(coords.MoveToDirection(direction)) > 0) {
                continue;
            }

            float distance = DistanceFrom(coords, direction);

            if (best == TankDirection.None || distance < bestDistance) {
                best = direction;
                bestDistance = distance;
            } 
        }

        TankDirectionTools.SetDirectionBit(ref usedDirections[coords.x, coords.y], best);
        return best;
    }

    private float DistanceFrom(IntCoords from, TankDirection direction) {
        from = from.MoveToDirection(direction);        return Vector.Distance(Vector.CoordsToPosition(from), Vector.CoordsToPosition(end));
    }

    private int Visited(IntCoords coords) {
        return visited[coords.x, coords.y];
    }

    private void Visit(IntCoords coords, byte value) {
        visited[coords.x, coords.y] = value;
    }
    private bool InvalidCoords(IntCoords coords, bool strict) {

        bool ob = coords.x < 0 || coords.x >= level.GetLength(0) ||
            coords.y < 0 || coords.y >= level.GetLength(1);

        if (!ob) {
            int limit = 2;
            if (strict) {
                limit = 1;
            }
            return visited[coords.x, coords.y] >= limit;
        }

        return true;
    }
}
