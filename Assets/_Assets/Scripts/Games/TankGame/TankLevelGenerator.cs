using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TankLevelGenerator : MonoBehaviour {

    #region fields
    [SerializeField]
    private GameObject cellPrefab;

    [SerializeField]
    private RectTransform cellParent, levelBackground;

    private Transform levelParent;

    private Vector2 area;

    private TankCell[,] cells;
    private bool[,] visited;

    private List<Step> steps;

    private int width, height;
    private float cellDiameter;

    private System.Random rnd;

    private bool generating = false;

    private struct Step {
        public Coords Coords;
        public TankCell.CellWall Wall;
        public override string ToString() {
            return Wall + " " + Coords;
        }
    }
    private struct Coords {
        public int X, Y;
        public override string ToString() {
            return "(" + X + ", " + Y + ")";
        }
    }

    private enum Direction { Start = -1, Up = 0, Right = 1, Down = 2, Left = 3 }
    #endregion

    private void Start() {
        levelParent = GameObject.FindGameObjectWithTag("Level").transform;
        rnd = new System.Random();
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            CreateLevel();
        }
    }

    public void CreateLevel() {

        if (generating) {
            return;
        }

        generating = true;

        Initialize();
        DFSGenerateMaze();
        BuildLevel();
    }

    #region Initialization
    private void Initialize() {
        ClearLevel();
        InitializeVariables();
        InitializeLevel();
    }

    private void InitializeVariables() {

        width = TankSettings.LevelWidth;
        height = TankSettings.LevelHeight;

        if (height > width) {
            int w = width;
            width = height;
            height = width;
        }

        cellDiameter = TankSettings.AreaWidth / width;

        visited = new bool[width, height];
        cells = new TankCell[width, height];

        steps = new List<Step>();
    }
    private void ClearLevel() {
        foreach (Transform child in cellParent) {
            Destroy(child.gameObject);
        }
    }

    private void InitializeLevel() {
        InitializeLevelArea();
        CreateCells();
    }

    private void InitializeLevelArea() {

        area = new Vector2(width, height) * (TankSettings.AreaWidth / width);

        levelBackground.sizeDelta = area;
        levelBackground.localPosition = Vector3.zero;

        float wOffset = area.x / 2;
        float hOffset = area.y / 2;

        Vector2[] corners = new Vector2[4];
        corners[0] = new Vector2(-wOffset, -hOffset);
        corners[1] = new Vector2(-wOffset, hOffset);
        corners[2] = new Vector2(wOffset, hOffset);
        corners[3] = new Vector2(wOffset, -hOffset);

        EdgeCollider2D col = levelBackground.GetComponent<EdgeCollider2D>();
        col.points = corners;
    }

    private void CreateCells() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                CreateCell(x, y);
            }
        }
    }
    private void CreateCell(int x, int y) {

        GameObject newCell = Instantiate(cellPrefab);

        newCell.name = "Cell (" + x + ", " + y + " )";

        cells[x, y] = newCell.GetComponent<TankCell>();
        cells[x, y].SetWalls(TankSettings.StartActive);

        RectTransform t = newCell.GetComponent<RectTransform>();

        t.SetParent(cellParent);
        t.localScale = new Vector3(1, 1, 1);
        t.sizeDelta = new Vector2(cellDiameter, cellDiameter);
        t.localPosition = CellPosition(x, y);
    }
    private Vector2 CellPosition(int x, int y) {

        Vector2 position = new Vector2(-area.x / 2, -area.y / 2);
        position.x += x * cellDiameter;
        position.y += y * cellDiameter;

        return position;
    }
    #endregion

    #region Generation
    private void DFSGenerateMaze() {
        visited[0, 0] = true;
        DFSGenerateMazeRecursive(new Coords(), StartDirection());
    }
    private Direction StartDirection() {
        if (rnd.Next(2) == 0) {
            return Direction.Right;
        }

        return Direction.Up;
    }

    private void DFSGenerateMazeRecursive(Coords coords, Direction direction) {

        Coords newCoords = MoveCoords(coords, direction);

        if (InvalidCoords(newCoords)) {
            return;
        }

        DFSMove(newCoords, direction);

        foreach (Direction dir in RandomDirection()) {
            DFSGenerateMazeRecursive(newCoords, dir);
        }
    }

    private void DFSMove(Coords coords, Direction direction) {

        Step step = new Step();

        visited[coords.X, coords.Y] = true;

        if (direction == Direction.Up) {

            step.Coords = MoveCoords(coords, Direction.Down);
            step.Wall = TankCell.CellWall.Top;

        } else if (direction == Direction.Right) {

            step.Coords = MoveCoords(coords, Direction.Left);
            step.Wall = TankCell.CellWall.Right;

        } else if (direction == Direction.Down) {

            step.Coords = coords;
            step.Wall = TankCell.CellWall.Top;

        } else if (direction == Direction.Left) {

            step.Coords = coords;
            step.Wall = TankCell.CellWall.Right;
        }

        steps.Add(step);
    }

    private bool InvalidCoords(Coords coords) {

        if (coords.X < 0 || coords.X >= width) {
            return true;
        }
        if (coords.Y < 0 || coords.Y >= height) {
            return true;
        }

        return visited[coords.X, coords.Y];
    }

    private Coords MoveCoords(Coords coords, Direction direction) {

        switch (direction) {
            case Direction.Up:
                coords.Y++;
                break;
            case Direction.Down:
                coords.Y--;
                break;
            case Direction.Right:
                coords.X++;
                break;
            case Direction.Left:
                coords.X--;
                break;
        }

        return coords;
    }

    private Direction[] RandomDirection() {

        Direction[] dir = new Direction[4];

        List<Direction> order = new List<Direction>();
        order.Add(Direction.Up);
        order.Add(Direction.Right);
        order.Add(Direction.Down);
        order.Add(Direction.Left);

        for (int i = 0; i < 4; i++) {
            int dirIndex = rnd.Next(order.Count);
            dir[i] = order[dirIndex];
            order.RemoveAt(dirIndex);
        }

        return dir;
    }
    #endregion

    #region Creation
    private void BuildLevel() {

        print("Step count: " + steps.Count);

        StartCoroutine(BuildCoroutine());
    }
    private IEnumerator BuildCoroutine() {

        float waitTime = TankSettings.GenerateTime / steps.Count;

        Stopwatch watch = new Stopwatch();

        float additional = 0;

        foreach (Step step in steps) {

            ProcessStep(step);

            if (waitTime == 0) {
                continue;
            } else if (waitTime + additional < Time.deltaTime) {
                additional += Time.deltaTime;
                continue;
            }

            yield return new WaitForSeconds(waitTime + additional);
            additional = 0;
        }

        generating = false;
    }
    private void ProcessStep(Step step) {

        print(step);

        cells[step.Coords.X, step.Coords.Y].SetWalls(true);
        cells[step.Coords.X, step.Coords.Y].DisableWall(step.Wall);
    }
    #endregion
}