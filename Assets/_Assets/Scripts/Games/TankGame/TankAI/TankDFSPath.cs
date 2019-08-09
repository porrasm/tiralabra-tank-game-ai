using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TankDFSPath : TankAIPathfinding {

    #region fields
    private byte[,] visited;

    private IntCoords start, end;

    private IntCoords coords;
    // Replace list
    public bool building;
    private Stack<IntCoords> route;

    private bool found;

    private FoundCondition foundCondition;
    #endregion

    public TankDFSPath(byte[,] level) :base(level) {

    }

    public override Vector[] FindPath(IntCoords start, IntCoords end, FoundCondition foundCondition) {
        this.start = start;
        this.end = end;

        this.foundCondition = foundCondition;

        visited = new byte[level.GetLength(0), level.GetLength(1)];
        route = new Stack<IntCoords>();

        found = false;

        Visit(start, 1);
        DFSRecursiveSearch(start, BestDirection2(start));

        // Replace this
        return route.Select(o => Vector.CoordsToPosition(o)).ToArray();
    }
    private void DFSRecursiveSearch(IntCoords coords, TankDirection direction) {

        if (foundCondition(coords)) {
            found = true;
            route.Push(coords);
            return;
        }

        if (found) {
            return;
        }

        IntCoords newCoords = coords.MoveToDirection(direction);


        if (InvalidCoords(newCoords, false)) {
            return;
        }

        Visit(newCoords, 1);
        route.Push(coords);   

        for (int i = 0; i < 8; i++) {
            TankDirection newDirection = BestDirection2(newCoords);
            if (newDirection == TankDirection.None) {
                Visit(newCoords, 2);
                break;
            } else if (newDirection == TankDirection.Backtrack) {
                break;
            }
            DFSRecursiveSearch(newCoords, newDirection);
        }

        if (!found) {
            route.Pop();
        }
    }

    private TankDirection BestDirection2(IntCoords coords) {

        TankDirection optDirection = TankDirection.None;
        float distance = float.MaxValue;
        IntCoords shortestCoords = new IntCoords(0, 0);

        byte allowed = level[coords.x, coords.y];

        for (int i = 0; i < 8; i++) {
            TankDirection direction = (TankDirection)i;

            if (!TankDirectionTools.AllowedDirection(allowed, direction)) {
                continue;
            }

            IntCoords newCoords = coords.MoveToDirection(direction);

            if (Visited(newCoords) > 0) {
                continue;
            }

            float newDistance = Vector.Distance(Vector.CoordsToPosition(newCoords), Vector.CoordsToPosition(end));

            if (newDistance < distance || (Visited(newCoords) == 0 && newDistance <= distance)) {
                distance = newDistance;
                optDirection = direction;
                shortestCoords = newCoords;
            }
        }

        return optDirection;
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
