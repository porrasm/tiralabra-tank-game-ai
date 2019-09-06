using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankLevelCellVisualizer {

    #region fields
    private static GameObject textPrefab;

    private static GameObject prev;
    private static TextMesh[,] texts;

    private static Transform cellParent;
    #endregion

    static TankLevelCellVisualizer() {
        textPrefab = Resources.Load<GameObject>("TankGame/DebugText");
        cellParent = GameObject.FindGameObjectWithTag("Level").transform.GetChild(2);
    }

    private static void InitTexts() {

        prev = new GameObject();

        texts = new TextMesh[TankSettings.LevelWidth, TankSettings.LevelHeight];

        for (int x = 0; x < TankSettings.LevelWidth; x++) {
            for (int y = 0; y < TankSettings.LevelHeight; y++) {
                texts[x, y] = CreateText(new IntCoords(x, y));
            }
        }
    }

    public static void VisualizeCells(byte[,] levelCellCounts) {

        if (!TankSettings.Debugging) {
            return;
        }

        if (prev == null) {
            InitTexts();
        }

        for (int x = 0; x < levelCellCounts.GetLength(0); x++) {
            for (int y = 0; y < levelCellCounts.GetLength(1); y++) {
                texts[x, y].text = CountString(new IntCoords(x, y), levelCellCounts[x, y]);
            }
        }
    }
    private static string CountString(IntCoords coords, int c) {
        return "" + c;
    }
    private static TextMesh CreateText(IntCoords coords) {

        GameObject newText = MonoBehaviour.Instantiate(textPrefab);

        newText.transform.SetParent(prev.transform);
        newText.transform.position = Vector.CoordsToPosition(coords).Vector3;

        newText.GetComponent<TextMesh>().text = "";

        return newText.GetComponent<TextMesh>();
        //newText.GetComponent<TextMesh>().text = coords + "\n" + count;
    }
}
