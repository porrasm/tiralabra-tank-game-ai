using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDFSPath {

    #region fields
    private byte[,] level;
    private byte[,] visited;

    private IntCoords start, end;

    private IntCoords coords;
    // Replace list
    public bool building;
    public Stack<IntCoords> route;
    #endregion

    public TankDFSPath() {
        level = TankLevelGenerator.Level;
        visited = new byte[level.GetLength(0), level.GetLength(1)];
    }

    public Stack<IntCoords> DFSPath(IntCoords start, IntCoords end) {

        this.start = start;
        this.end = end;

        route = new Stack<IntCoords>();

        Visit(start, 1);
        //DFSSearch();

        return route;
    }

    public IEnumerator DFSSearchSafe(IntCoords start, IntCoords end) {

        building = true;

        this.start = start;
        this.end = end;

        route = new Stack<IntCoords>();

        Visit(start, 1);

        route.Push(start);

        coords = start;

        while (true) {

            yield return null;

            Debug.DrawLine(Vector.ToVector3(Vector.CoordsToPosition(coords)), Vector.ToVector3(Vector.CoordsToPosition(coords)) + Vector3.one * 0.2f);

            Move();

            MonoBehaviour.print("Current pos: " + coords);

            if (Finished()) {
                break;
            }

            if (!CanMove(coords)) {
                Backtrack();
            }
        }

        building = false;
        MonoBehaviour.print("Done");
    }

    private void Backtrack() {

        MonoBehaviour.print("Backtrack");

        while (true) {
            Visit(route.Pop(), 2);
            if (route.Count == 0) {
                return;
            }
            if (CanMove(route.Peek())) {
                return;
            }
        }
    }

    private bool Finished() {
        MonoBehaviour.print(coords + " == " + end);
        return coords == end;
    }

    private void Move() {

        TankDirection best = BestDirection(coords);

        MonoBehaviour.print("Moving " + best);

        coords = coords.MoveToDirection(best);
        route.Push(coords);
        Visit(coords, 1);
    }

    private bool CanMove(IntCoords coords) {

        byte allowed = level[coords.x, coords.y];

        for (int i = 0; i < 8; i++) {
            TankDirection direction = (TankDirection)i;

            if (!TankDirectionTools.AllowedDirection(allowed, direction)) {
                MonoBehaviour.print("Not allowed dir: " + Convert.ToString(allowed, 2) + ", dir: " + direction);
                continue;
            }

            IntCoords newCoords = coords.MoveToDirection(direction);

            if (!InvalidCoords(newCoords, true)) {
                MonoBehaviour.print("Invalid coords");
                return true;
            }
        }

        MonoBehaviour.print("Cant move");
        return false;
    }


    private TankDirection BestDirection(IntCoords coords) {

        TankDirection optDirection = TankDirection.Start;
        float distance = float.MaxValue;

        byte allowed = level[coords.x, coords.y];

        for (int i = 0; i < 8; i++) {
            TankDirection direction = (TankDirection)i;

            if (!TankDirectionTools.AllowedDirection(allowed, direction)) {
                MonoBehaviour.print(direction + " was not allowed from " + coords + ", allowed: " + Convert.ToString(allowed, 2));
                continue;
            }

            IntCoords newCoords = coords.MoveToDirection(direction);

            if (InvalidCoords(newCoords, false)) {
                MonoBehaviour.print("Moving to " + direction + " was not allowed from " + coords + ", allowed: " + Convert.ToString(allowed, 2));
                continue;
            }

            float newDistance = Vector.Distance(Vector.CoordsToPosition(newCoords), Vector.CoordsToPosition(end));

            MonoBehaviour.print("New distance: " + newDistance + ", prev distance: " + distance);

            if (newDistance < distance) {
                distance = newDistance;
                optDirection = direction;
            }
        }

        return optDirection;
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
            MonoBehaviour.print(visited[coords.x, coords.y] + " >= " + limit);
            return visited[coords.x, coords.y] >= limit;
        }

        MonoBehaviour.print("out of bounds");
        return true;
    }
}
