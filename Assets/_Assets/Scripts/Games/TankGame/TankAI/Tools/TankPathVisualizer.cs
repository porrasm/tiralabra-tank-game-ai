using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPathVisualizer : MonoBehaviour {

    #region fields
    TankDFSPath dfs;
    LineRenderer line;
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

        if (Input.GetKeyDown(KeyCode.Space)) {
            UpdateRoute();
        }

        DrawRoute();
    }

    private void UpdateRoute() {

        if (dfs == null) {
            return;
        }

        IntCoords coords = MouseToCoords();
        StartCoroutine(dfs.DFSSearchSafe(new IntCoords(0, 0), coords));



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
        if (dfs.building) {
            return;
        }

        dfs.building = true;

        line.SetPositions(RouteToPos(dfs.route));
    }
    private Vector3[] RouteToPos(Stack<IntCoords> route) {

        Vector3[] pos = new Vector3[route.Count];

        for (int i = 0; i < pos.Length; i++) {
            pos[i] = Vector.ToVector3(Vector.CoordsToPosition(route.Pop()));
        }

        print("Building line: " + pos.Length);

        return pos;
    }
}
