using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Most things breaks if width != height, fix later :):)
public class TankLevelGenerator : MonoBehaviour {

    #region fields
    public static TankLevelGenerator Instance { get; private set; }

    private TankMazeGenerator generator;
    private List<Step> steps;
    private TankCell[,] cells;
    private Vector2 area;

    private int width;
    private int height;

    [SerializeField]
    private GameObject cellPrefab;

    [SerializeField]
    private Transform cellParent, levelFloor;

    private Transform levelParent;

    public bool Building { get; set; }

    private byte[,] level;

    public byte[,] Level { get => level; private set => level = value; }

    public struct Step {
        public IntCoords Coords;
        public TankCell.CellWall Wall;
        public bool Silent;
        public override string ToString() {
            return Wall + " " + Coords;
        }
    }

    #endregion

    #region Basic
    private void Awake() {

        if (Instance == null) {
            Instance = this;
        } else {
            UnityEngine.Debug.LogError("Instance is already set.");
            Destroy(this);
        }

        generator = new TankMazeGenerator();
    }
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
        generator.GenerateMaze(out steps, out level);
    }
    public void BuildGeneratedLevel() {

        if (Building) {
            return;
        }

        Initialize();

        Building = true;

        BuildLevel();
    }

    #endregion

    #region Initialization
    private void Initialize() {
        ClearLevel();
        InitializeVariables();
        InitializeLevel();
    }

    private void InitializeVariables() {

        width = TankSettings.LevelWidth;
        height = TankSettings.LevelHeight;

        cells = new TankCell[width, height];
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
    }

    private void InitializeLevelArea() {
        levelFloor.localScale = new Vector3(0.1f * height, 1, 0.1f * width);
        levelFloor.position = new Vector3(0.5f * width, levelFloor.position.y, 0.5f * height);
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
        return new Vector3(x, 0, y);
    }
    #endregion
   
    #region Building
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

        TankCell cell = cells[step.Coords.x, step.Coords.y];

        if (cell == null) {
            return true;
        }

        cell.SetWalls(true);
        cell.DisableWall(step.Wall);

        return step.Silent;
    }
    #endregion
}