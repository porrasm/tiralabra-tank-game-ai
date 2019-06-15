using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TankLevelGenerator : MonoBehaviour {

    #region fields
    [SerializeField]
    private GameObject cellPrefab;

    [SerializeField]
    private Transform cellParent, levelFloor;

    private Transform levelParent;

    private Vector2 area;

    private TankCell[,] cells;
    private bool[,] visited;

    private List<Step> steps;

    private int width, height;
    //private float cellDiameter;

    private System.Random rnd;

    public bool Building { get; set; }

    private struct Step {
        public Coords Coords;
        public TankCell.CellWall Wall;
        public bool Silent;
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
            GenerateLevel();
        }
    }

    public void GenerateLevel() {
        Initialize();
        DFSGenerateMaze();
        CleanLevel();    
    }
    public void BuildGeneratedLevel() {

        if (Building) {
            return;
        }

        Building = true;

        BuildLevel();
    }

    private void AddStep(int x, int y, TankCell.CellWall wall, bool silent) {
        Step step = new Step() { Coords = new Coords() { X = x, Y = y }, Wall = wall, Silent = silent };
        steps.Add(step);
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

        //cellDiameter = TankSettings.AreaWidth / width;

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
        InitializeCamera();
        SetSpawns();
        CreateCells();
        RemoveEdgeWalls();
    }

    private void InitializeLevelArea() {
        levelFloor.localScale = new Vector3(0.1f * width, 1, 0.1f * height);
    }
    private void InitializeCamera() {

        Vector3 position = new Vector3(1.0f * width / 2, 10, 1.0f * height / 2);
        float size;

        if (width > height) {
            size = position.x * TankSettings.CameraSizeFactorX;
        } else {
            size = position.z * TankSettings.CameraSizeFactorY;
        }

        Camera.main.transform.position = position;
        Camera.main.orthographicSize = size;
    }

    private void SetSpawns() {

        Transform spawns = GameObject.FindGameObjectWithTag("Respawn").transform;

        for (int i = 0; i < spawns.childCount; i++) {
            spawns.GetChild(i).position = SpawnPosition(i);
            spawns.GetChild(i).LookAt(new Vector3(0.5f * width, 0, 0.5f * height));
        }
    }
    private Vector3 SpawnPosition(int index) {

        float offset = 0.5f;

        switch (index) {
            case 0:
                return new Vector3(offset, 0, offset);
            case 1:
                return new Vector3(width - offset, 0, height - offset);
            case 2:
                return new Vector3(offset, 0, height - offset);
            case 3:
                return new Vector3(width - offset, 0, offset);
            case 4:
                return new Vector3(offset * width, 0, offset);
            case 5:
                return new Vector3(offset * width, 0, height - offset);
            case 6:
                return new Vector3(width - offset, 0, offset * height);
            case 7:
                return new Vector3(offset, 0, offset * height);
        }

        return Vector3.zero;
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

        Transform t = newCell.transform;

        t.SetParent(cellParent);
        t.localScale = new Vector3(1, 1, 1);
        t.localPosition = CellPosition(x, y);
    }
    private Vector3 CellPosition(int x, int y) {
        // X & Y are swapped for some reason, works now
        return new Vector3(y, 0, x);
    }

    private void RemoveEdgeWalls() {
        for (int x = 0; x < width; x++) {
            AddStep(x, height - 1, TankCell.CellWall.Top, true);
        }
        for (int y = 0; y < height; y++) {
            AddStep(width - 1, y, TankCell.CellWall.Right, true);
        }
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

    #region Cleaning
    private void CleanLevel() {
        CleanSpawns();
        ThinLevel();
    }

    private void CleanSpawns() {

        int cleanW = width - 2;
        int cleanH = height - 2;
        int cleanWHalf = width / 2 - 1;
        int cleanHHalf = height / 2 - 1;

        CleanArea(0, 0);
        CleanArea(cleanW, cleanH);
        CleanArea(cleanW, 0);
        CleanArea(0, cleanH);

        CleanArea(cleanWHalf, 0);
        CleanArea(cleanWHalf, cleanH);
        CleanArea(0, cleanHHalf);
        CleanArea(cleanW, cleanHHalf);
    }
    private void CleanArea(int x, int y) {
        for (int i = 0; i < 1; i++) {
            for (int j = 0; j < 1; j++) {
                AddStep(x + i, y + j, TankCell.CellWall.Both, false);
            }
        }
    }


    private void ThinLevel() {

        for (int i = 0; i < width; i++) {

            int amount = rnd.Next(0, 4) + 2;
            bool deleting = Random.value > TankSettings.CleanProbability;
            int n = 2;

            for (int j = 0; j < height; j++) {

                if (deleting) {

                    AddStep(i, j, TankCell.CellWall.Both, false);

                    n++;
                    if (n >= amount) {
                        deleting = false;
                    }
                } else if (Random.value > TankSettings.CleanProbability) {
                    AddStep(i, j, TankCell.CellWall.Both, false);
                    deleting = true;
                }
            }
        }
    }
    #endregion

    #region Creation
    private void BuildLevel() {
        StartCoroutine(BuildCoroutine());
    }
    private IEnumerator BuildCoroutine() {

        float waitTime = TankSettings.GenerateTime / steps.Count;

        Stopwatch watch = new Stopwatch();

        float additional = 0;

        foreach (Step step in steps) {

            if (ProcessStep(step)) {
                continue;
            }

            if (waitTime == 0) {
                continue;
            } else if (waitTime + additional < Time.deltaTime) {
                additional += Time.deltaTime;
                continue;
            }

            yield return new WaitForSeconds(waitTime + additional);
            additional = 0;
        }

        Building = false;
    }
    private bool ProcessStep(Step step) {

        TankCell cell = cells[step.Coords.X, step.Coords.Y];

        if (cell == null) {
            return true;
        }

        cell.SetWalls(true);
        cell.DisableWall(step.Wall);

        return step.Silent;
    }
    #endregion
}