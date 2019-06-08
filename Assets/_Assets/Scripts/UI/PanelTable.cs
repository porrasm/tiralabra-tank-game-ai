using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelTable {

    private static GameObject tablePrefab;

    public static GameObject CreateTable(Transform parent, string[,] content, Vector3 size, bool cellSize) {

        float cellWidth, cellHeight, tableWidth, tableHeight;
        SetSizes(out cellWidth, out cellHeight, out tableWidth, out tableHeight, size, content, cellSize);

        GameObject table = new GameObject();
        table.name = "Table";
        table.transform.localScale = new Vector3(tableWidth, tableHeight, 1);
        RectTransform rect = table.AddComponent<RectTransform>();
        table.transform.SetParent(parent);
        Image background = table.AddComponent<Image>();
       

        for (int y = 0; y < content.GetLength(1); y++) {

            GameObject row = new GameObject();
            row.name = "Row " + y;
            row.AddComponent<RectTransform>();
            row.transform.SetParent(table.transform);
            row.transform.localPosition = new Vector3(0, -y * cellHeight, 0);

            for (int x = 0; x < content.GetLength(0); x++) {

                Text text = CreateTextObject(content[x, y], cellWidth, cellHeight);
                if (y == 0) {
                    text.fontStyle = FontStyle.Bold;
                }

                text.transform.SetParent(row.transform);
                text.transform.localPosition = new Vector3(x * cellWidth, 0, 0);   
            }
        }

        return table;
    }
    private static Text CreateTextObject(string content, float width, float height) {
        GameObject textObject = new GameObject();
        textObject.name = "Cell";
        RectTransform rect = textObject.AddComponent<RectTransform>();
        Text text = textObject.AddComponent<Text>();

        rect.sizeDelta = new Vector2(width, height);

        text.text = content;
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = 24;

        return text;
    }

    private static void SetSizes(out float cellWidth, out float cellHeight, out float tableWidth, out float tableHeight, Vector3 size, string[,] content, bool cellSize) {
        if (cellSize) {
            cellWidth = size.x;
            cellHeight = size.y;

            tableWidth = content.GetLength(0) * cellWidth;
            tableHeight = content.GetLength(1) * cellHeight;
        } else {
            tableWidth = size.x;
            tableHeight = size.y;

            cellWidth = tableWidth / content.GetLength(1);
            cellHeight = tableHeight / content.GetLength(0);
        }
    }
}
