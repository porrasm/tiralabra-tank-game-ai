using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMazeGenerator {

    #region fields
    private List<TankLevelGenerator.Step> steps;
    private byte[,] level;
    private bool[,] visited;

    private System.Random rnd;
    private int width;
    private int height;
    #endregion

    public TankMazeGenerator() {
        rnd = new System.Random();
    }

    public void LevelFromSteps(out byte[,] oLevel, List<TankLevelGenerator.Step> levelSteps) {

        // oLevel = null;

        InitializeVariables();
        steps = levelSteps;
        SetLevelArray();

        oLevel = level;

        NullifyVariables();
    }
    public void GenerateMaze(out List<TankLevelGenerator.Step> oSteps, out byte[,] oLevel) {

        InitializeVariables();
        Generate();

        SetLevelArray();

        oSteps = steps;
        oLevel = level;

        NullifyVariables();
    }

    private void Generate() {
        DFSGenerateMaze();
        CleanLevel();   
    }

    private void InitializeVariables() {

        width = TankSettings.LevelWidth;
        height = TankSettings.LevelHeight;

        visited = new bool[width, height];

        steps = new List<TankLevelGenerator.Step>();
        level = new byte[width, height];
    }
    private void NullifyVariables() {
        steps = null;
        level = null;
        visited = null;
    }

    private void AddStep(int x, int y, TankCell.CellWall wall, bool silent) {
        TankLevelGenerator.Step step = new TankLevelGenerator.Step() { Coords = new IntCoords(x, y), Wall = wall, Silent = silent };
        steps.Add(step);
    }

    #region Generation
    private void DFSGenerateMaze() {
        visited[0, 0] = true;
        DFSGenerateMazeRecursive(new IntCoords(), StartDirection());
    }
    private TankDirection StartDirection() {
        if (rnd.Next(2) == 0) {
            return TankDirection.Right;
        }

        return TankDirection.Up;
    }

    private void DFSGenerateMazeRecursive(IntCoords coords, TankDirection direction) {

        IntCoords newCoords = coords.MoveToDirection(direction);

        if (InvalidIntCoords(newCoords)) {
            return;
        }

        DFSMove(newCoords, direction);

        foreach (TankDirection dir in RandomDirection()) {
            DFSGenerateMazeRecursive(newCoords, dir);
        }
    }

    private void DFSMove(IntCoords coords, TankDirection direction) {

        TankLevelGenerator.Step step = new TankLevelGenerator.Step();

        visited[coords.x, coords.y] = true;

        if (direction == TankDirection.Up) {

            step.Coords = coords.MoveToDirection(TankDirection.Down);
            step.Wall = TankCell.CellWall.Top;

        } else if (direction == TankDirection.Right) {

            step.Coords = coords.MoveToDirection(TankDirection.Left);
            step.Wall = TankCell.CellWall.Right;

        } else if (direction == TankDirection.Down) {

            step.Coords = coords;
            step.Wall = TankCell.CellWall.Top;

        } else if (direction == TankDirection.Left) {

            step.Coords = coords;
            step.Wall = TankCell.CellWall.Right;
        }

        steps.Add(step);
    }

    private bool InvalidIntCoords(IntCoords coords) {

        if (coords.x < 0 || coords.x >= width) {
            return true;
        }
        if (coords.y < 0 || coords.y >= height) {
            return true;
        }

        return visited[coords.x, coords.y];
    }

    private TankDirection[] RandomDirection() {

        TankDirection[] dir = new TankDirection[4];

        List<TankDirection> order = new List<TankDirection>();
        order.Add(TankDirection.Up);
        order.Add(TankDirection.Right);
        order.Add(TankDirection.Down);
        order.Add(TankDirection.Left);

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
        RemoveEdgeWalls();
        CleanSpawns();
        ThinLevel();
    }

    private void RemoveEdgeWalls() {
        for (int x = 0; x < width; x++) {
            AddStep(x, height - 1, TankCell.CellWall.Top, true);
        }
        for (int y = 0; y < height; y++) {
            AddStep(width - 1, y, TankCell.CellWall.Right, true);
        }
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
            bool deleting = RNG.Float > TankSettings.CleanProbability;
            int n = 2;

            for (int j = 0; j < height; j++) {

                if (deleting) {

                    AddStep(i, j, TankCell.CellWall.Both, false);

                    n++;
                    if (n >= amount) {
                        deleting = false;
                    }
                } else if (RNG.Float > TankSettings.CleanProbability) {
                    AddStep(i, j, TankCell.CellWall.Both, false);
                    deleting = true;
                }
            }
        }
    }
    #endregion

    #region LevelField
    private void SetLevelArray() {
        SetBasicDirections();
        SetAdvancedDirections();
    }

    private void SetBasicDirections() {

        foreach (TankLevelGenerator.Step step in steps) {

            if (step.Wall == TankCell.CellWall.Top || step.Wall == TankCell.CellWall.Both) {
                LinkDirection(step.Coords.x, step.Coords.y, TankDirection.Up);
            }
            if (step.Wall == TankCell.CellWall.Right || step.Wall == TankCell.CellWall.Both) {
                LinkDirection(step.Coords.x, step.Coords.y, TankDirection.Right);
            }
        }
    }

    private void SetAdvancedDirections() {

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {

                byte b = level[x, y];

                if (TankDirectionTools.AllowedDirection(b, TankDirection.Up) && TankDirectionTools.AllowedDirection(b, TankDirection.Right)) {

                    IntCoords moved = new IntCoords(x, y).MoveToDirection(TankDirection.UpRight);
                    byte b2 = level[moved.x, moved.y];

                    if (TankDirectionTools.AllowedDirection(b2, TankDirection.Down) && TankDirectionTools.AllowedDirection(b2, TankDirection.Left)) {
                        LinkDirection(x, y, TankDirection.UpRight);
                    }
                }

                if (TankDirectionTools.AllowedDirection(b, TankDirection.Down) && TankDirectionTools.AllowedDirection(b, TankDirection.Right)) {

                    IntCoords moved = new IntCoords(x, y).MoveToDirection(TankDirection.DownRight);
                    byte b2 = level[moved.x, moved.y];

                    if (TankDirectionTools.AllowedDirection(b2, TankDirection.Up) && TankDirectionTools.AllowedDirection(b2, TankDirection.Left)) {
                        LinkDirection(x, y, TankDirection.DownRight);
                    }
                }
            }
        }
    }

    private void LinkDirection(int x, int y, TankDirection direction) {

        int newX = x;
        int newY = y;
        TankDirection newDirection = direction;

        IncrementAndSwitch(ref newX, ref newY, ref newDirection);

        if (!PossibleCoords(x, y) || !PossibleCoords(newX, newY)) {
            return;
        }

        TankDirectionTools.SetDirectionBit(ref level[x, y], direction);
        TankDirectionTools.SetDirectionBit(ref level[newX, newY], newDirection);
    }
    private void IncrementAndSwitch(ref int x, ref int y, ref TankDirection direction) {

        switch (direction) {
            case TankDirection.Up:
                y++;
                direction = TankDirection.Down;
                break;
            case TankDirection.Right:
                x++;
                direction = TankDirection.Left;
                break;
            case TankDirection.Down:
                y--;
                direction = TankDirection.Up;
                break;
            case TankDirection.Left:
                x--;
                direction = TankDirection.Right;
                break;
            case TankDirection.UpRight:
                x++;
                y++;
                direction = TankDirection.DownLeft;
                break;
            case TankDirection.DownRight:
                x++;
                y--;
                direction = TankDirection.UpLeft;
                break;
            case TankDirection.DownLeft:
                x--;
                y--;
                direction = TankDirection.UpRight;
                break;
            case TankDirection.UpLeft:
                x--;
                y++;
                direction = TankDirection.DownRight;
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
    #endregion
}
