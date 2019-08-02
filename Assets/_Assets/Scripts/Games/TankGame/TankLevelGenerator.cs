using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Most things breaks if width != height, fix later :):)
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

    private System.Random rnd = new System.Random();

    public bool Building { get; set; }
    public static byte[,] Level { get; private set; }

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

    private enum Direction { Start = -1, Up = 0, Right = 1, Down = 2, Left = 3, UpRight = 4, DownRight = 5, DownLeft = 6, UpLeft = 7 }
    #endregion

    #region Basic
    private void Start() {
        levelParent = GameObject.FindGameObjectWithTag("Level").transform;
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            GenerateLevel();
            BuildGeneratedLevel();
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
    #endregion

    #region Initialization
    private void Initialize() {
        ClearLevel();
        InitializeVariables();
        InitializeLevel();
    }

    private void InitializeVariables() {

        // Values are swapped
        width = TankSettings.LevelWidth;
        height = TankSettings.LevelHeight;

        //cellDiameter = TankSettings.AreaWidth / width;

        visited = new bool[width, height];
        cells = new TankCell[width, height];

        steps = new List<Step>();

        Level = new byte[width, height];
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

    // Initialization needs to swap width & height
    private void InitializeLevelArea() {
        levelFloor.localScale = new Vector3(0.1f * height, 1, 0.1f * width);
    }
    private void InitializeCamera() {

        Vector3 position = new Vector3(1.0f * height / 2, 10, 1.0f * width / 2);
        float size;

        if (height > width) {
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

    #region LevelField
    private void SetLevelArray() {
        SetBasicDirections();
        SetAdvancedDirections();

        print("Set level array");

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                print("(" + x + ", " + y + ") " + Convert.ToString(Level[x, y], 2));
            }
        }
    }

    private void SetBasicDirections() {

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {

                TankCell cell = cells[x, y];

                if (cell == null || !cell.Top) {
                    LinkDirection(x, y, Direction.Up);
                }
                if (cell == null || !cell.Right) {
                    LinkDirection(x, y, Direction.Right);
                }
            }
        }
    }
    private void SetAdvancedDirections() {

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {

                byte b = Level[x, y];
               
                if (PossibleDirection(b, Direction.Up) && PossibleDirection(b, Direction.Right)) {
                    LinkDirection(x, y, Direction.UpRight);
                }
                if (PossibleDirection(b, Direction.Down) && PossibleDirection(b, Direction.Right)) {
                    LinkDirection(x, y, Direction.DownRight);
                }
                if (PossibleDirection(b, Direction.Down) && PossibleDirection(b, Direction.Left)) {
                    LinkDirection(x, y, Direction.DownLeft);
                }
                if (PossibleDirection(b, Direction.Up) && PossibleDirection(b, Direction.Left)) {
                    LinkDirection(x, y, Direction.UpLeft);
                }
            }
        }
    }

    private void LinkDirection(int x, int y, Direction direction) {

        SetDirectionBit(ref Level[x, y], direction);
        IncrementAndSwitch(ref x, ref y, ref direction);
        
        if (PossibleCoords(x, y)) {
            SetDirectionBit(ref Level[x, y], direction);
        }
    }
    private void IncrementAndSwitch(ref int x, ref int y, ref Direction direction) {

        switch (direction) {
            case Direction.Up:
                y++;
                direction = Direction.Down;
                break;
            case Direction.Right:
                x++;
                direction = Direction.Left;
                break;
            case Direction.Down:
                y--;
                direction = Direction.Up;
                break;
            case Direction.Left:
                x--;
                direction = Direction.Right;
                break;
            case Direction.UpRight:
                x++;
                y++;
                direction = Direction.DownLeft;
                break;
            case Direction.DownRight:
                x++;
                y--;
                direction = Direction.UpLeft;
                break;
            case Direction.DownLeft:
                x--;
                y--;
                direction = Direction.UpRight;
                break;
            case Direction.UpLeft:
                x--;
                y++;
                direction = Direction.DownRight;
                break;
        }
    }

    
    private bool PossibleCoords(int x, int y) {
        if (x < 0 || x >= width) {
            return false;
        }
        if (y < 0 || y >= height) {
            return false;
        }

        return true;
    }

    private bool PossibleDirection(byte b, Direction direction) {
        return (b & DirectionToByte(direction)) != 0;
    }

    private void SetDirectionBit(ref byte value, Direction direction) {
        byte bit = DirectionToByte(direction);
        value = (byte)(value | bit);
    }
    private byte DirectionToByte(Direction direction) {
        return (byte)(1 << (int)direction);
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
            bool deleting = UnityEngine.Random.value > TankSettings.CleanProbability;
            int n = 2;

            for (int j = 0; j < height; j++) {

                if (deleting) {

                    AddStep(i, j, TankCell.CellWall.Both, false);

                    n++;
                    if (n >= amount) {
                        deleting = false;
                    }
                } else if (UnityEngine.Random.value > TankSettings.CleanProbability) {
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

        SetLevelArray();

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