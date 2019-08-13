using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class is used for testing/debugging the pathfinding and level generation.
/// </summary>
public class TankPathVisualizer : MonoBehaviour {

    #region fields
    private TankDFSPath dfs;
    private TankAStarPath aStar;
    private LineRenderer line;

    private IntCoords start;

    public byte[,] level;
    #endregion

    private void Init() {
        dfs = new TankDFSPath(TankLevelGenerator.Instance.Level);
        aStar = new TankAStarPath(TankLevelGenerator.Instance.Level);
        line = GetComponent<LineRenderer>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            dfs = null;
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            level = TankLevelGenerator.Instance.Level;
            Init();
        }
        if (Input.GetKeyDown(KeyCode.X)) {

            Debug.Log("Creating test level");

            List<TankLevelGenerator.Step> steps = new List<TankLevelGenerator.Step>();

            for (int x = 0; x < 10; x++) {
                for (int y = 0; y < 10; y++) {
                    TankLevelGenerator.Step step = new TankLevelGenerator.Step();
                    step.Coords = new IntCoords(x, y);
                    step.Wall = TankCell.CellWall.Both;
                    steps.Add(step);
                }
            }

            Debug.Log("Step count: " + steps.Count);

            TankMazeGenerator g = new TankMazeGenerator();
            g.LevelFromSteps(out level, steps);

            dfs = new TankDFSPath(level);
            aStar = new TankAStarPath(level);
            line = GetComponent<LineRenderer>();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            Comparison();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            start = Vector.PositionToCoords(Vector.FromVector3(world));
        }
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            UpdateRouteAStar();
        }
        if (Input.GetKey(KeyCode.Mouse1)) {
            UpdateRouteDFS();
        }

        DrawLevel(level);
    }

    public void DrawLevel(byte[,] level) {

        if (level == null) {
            return;
        }

        for (int x = 0; x < 10; x++) {
            for (int y = 0; y < 10; y++) {
                DrawAvailable(new IntCoords(x, y), level[x, y]);
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
                dfs.FindPath(start, new IntCoords(x, y));
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

        DrawRoute(dfs.FindPath(start, coords));
    }
    private void UpdateRouteAStar() {

        if (aStar == null) {
            return;
        }

        IntCoords coords = MouseToCoords();

        Vector[] route = aStar.FindPath(start, coords);

        DrawRoute(route);
    }

    private IntCoords MouseToCoords() {

        Vector3 mouse = Input.mousePosition;
        Vector3 world = Camera.main.ScreenToWorldPoint(mouse);

        UnityEngine.Debug.DrawLine(Vector3.zero, world);

        return Vector.PositionToCoords(Vector.FromVector3(world));
    }

    private Vector3[] RouteToPos(Stack<IntCoords> route) {

        Vector3[] pos = new Vector3[route.Count];

        for (int i = 0; i < pos.Length; i++) {
            pos[pos.Length - 1 - i] = Vector.ToVector3(Vector.CoordsToPosition(route.Pop()));
        }

        return pos;
    }

    public static void DrawRoute(Vector[] positions) {
        DrawRoute(positions.Select(o => o.Vector3).ToArray());
    }
    public static void DrawRoute(Vector3[] positions) {
        LineRenderer line = GameObject.FindGameObjectWithTag("Scripts").GetComponent<LineRenderer>();
        if (line == null) {
            return;
        }

        line.positionCount = positions.Length;
        line.SetPositions(positions);
    }
}
