using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPathVisualizer : MonoBehaviour {

    #region fields
    TankDFSPath dfs;
    LineRenderer line;

    private IntCoords start;
    #endregion

    private void Init() {
        dfs = new TankDFSPath();
        line = GetComponent<LineRenderer>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            dfs = null;
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            Init();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            start = Vector.PositionToCoords(Vector.FromVector3(world));
        }
        if (Input.GetKey(KeyCode.Mouse0)) {
            UpdateRoute();
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
        Debug.DrawLine(Vector.ToVector3(Vector.CoordsToPosition(a)), Vector.ToVector3(Vector.CoordsToPosition(b)));
    }

    private void UpdateRoute() {

        if (dfs == null) {
            return;
        }

        IntCoords coords = MouseToCoords();
        //StartCoroutine(dfs.DFSSearchSafe(new IntCoords(0, 0), coords));

        dfs.DFSRecursive(start, coords);

        DrawRoute();

        //DrawRoute(route);
    }

    private IntCoords MouseToCoords() {

        Vector3 mouse = Input.mousePosition;
        Vector3 world = Camera.main.ScreenToWorldPoint(mouse);

        Debug.DrawLine(Vector3.zero, world);

        return Vector.PositionToCoords(Vector.FromVector3(world));
    }

    private void DrawRoute() {

        if (dfs == null) {
            return;
        }
        //if (dfs.building) {
        //    return;
        //}

        //dfs.building = true;

        Vector3[] positions = RouteToPos(dfs.route);
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
