using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TankPathVisualizer : MonoBehaviour {

    #region fields
    TankDFSPath dfs;
    TankAStarPath aStar;
    LineRenderer line;

    private IntCoords start;
    #endregion

    private void Init() {
        dfs = new TankDFSPath();
        aStar = new TankAStarPath(TankLevelGenerator.Level);
        line = GetComponent<LineRenderer>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            dfs = null;
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            Init();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            Comparison();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            start = Vector.PositionToCoords(Vector.FromVector3(world));
        }
        if (Input.GetKey(KeyCode.Mouse0)) {
            UpdateRouteAStar();
        }
        if (Input.GetKey(KeyCode.Mouse1)) {
            UpdateRouteDFS();
        }

        DrawLevel();
    }

    private void DrawLevel() {

        if (TankLevelGenerator.Level == null) {
            return;
        }

        for (int x = 0; x < 10; x++) {
            for (int y = 0; y < 10; y++) {
                DrawAvailable(new IntCoords(x, y), TankLevelGenerator.Level[x, y]);
            }
        }

    }
    private void DrawAvailable(IntCoords coords, byte allowed) {

        for (int i = 0; i < 8; i++) {

            TankDirection direction = (TankDirection)i;

            if (TankDirectionTools.AllowedDirection(allowed, direction)) {
                IntCoords newCoords = coords.MoveToDirection(direction);
                DrawLine(coords, newCoords);
            }
        }

    }
    private void DrawLine(IntCoords a, IntCoords b) {
        UnityEngine.Debug.DrawLine(Vector.ToVector3(Vector.CoordsToPosition(a)), Vector.ToVector3(Vector.CoordsToPosition(b)));
    }

    private void Comparison() {

        IntCoords start = new IntCoords();

        var watch = System.Diagnostics.Stopwatch.StartNew();

        for (int x = 1; x < TankSettings.LevelWidth; x++) {
            for (int y = 1; y < TankSettings.LevelHeight; y++) {
                dfs.DFSRecursive(start, new IntCoords(x, y));
            }
        }

        watch.Stop();

        long elapsedDFS = watch.ElapsedMilliseconds;

        print("For DFS it took " + elapsedDFS + " millisecond to complete");

        watch = System.Diagnostics.Stopwatch.StartNew();
        for (int x = 1; x < TankSettings.LevelWidth; x++) {
            for (int y = 1; y < TankSettings.LevelHeight; y++) {
                aStar.FindPath(start, new IntCoords(x, y));
            }
        }

        watch.Stop();

        long elapsedAStar = watch.ElapsedMilliseconds;

        print("For A* it took " + elapsedAStar + " millisecond to complete");

        print("A* / DFS = " + (1.0 * elapsedAStar / elapsedDFS));
        print("DFS / A* = " + (1.0 * elapsedDFS / elapsedAStar));
    }

    private void UpdateRouteDFS() {

        if (dfs == null) {
            return;
        }

        IntCoords coords = MouseToCoords();
        //StartCoroutine(dfs.DFSSearchSafe(new IntCoords(0, 0), coords));

        dfs.DFSRecursive(start, coords);

        DrawRoute(RouteToPos(dfs.route));

        //DrawRoute(route);
    }
    private void UpdateRouteAStar() {

        if (aStar == null) {
            return;
        }

        IntCoords coords = MouseToCoords();
        //StartCoroutine(dfs.DFSSearchSafe(new IntCoords(0, 0), coords));

        IntCoords[] route = aStar.FindPath(start, coords);

        DrawRoute(route.Select(o => Vector.ToVector3(Vector.CoordsToPosition(o))).ToArray());

        //DrawRoute(route);
    }

    private IntCoords MouseToCoords() {

        Vector3 mouse = Input.mousePosition;
        Vector3 world = Camera.main.ScreenToWorldPoint(mouse);

        UnityEngine.Debug.DrawLine(Vector3.zero, world);

        return Vector.PositionToCoords(Vector.FromVector3(world));
    }

    private void DrawRoute(Vector3[] positions) {

        if (dfs == null) {
            return;
        }
        //if (dfs.building) {
        //    return;
        //}

        //dfs.building = true;

        line.positionCount = positions.Length;
        line.SetPositions(positions);
    }
    private Vector3[] RouteToPos(Stack<IntCoords> route) {

        Vector3[] pos = new Vector3[route.Count];

        for (int i = 0; i < pos.Length; i++) {
            pos[pos.Length - 1 - i] = Vector.ToVector3(Vector.CoordsToPosition(route.Pop()));
        }

        return pos;
    }
}
