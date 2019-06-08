using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colors {

    public static Color GetBasicColor(PlayerColor pcolor) {
        return GetBasicColor(pcolor, 255);
    }

    public static Color GetBasicColor(PlayerColor pcolor, byte alpha) {

        Color32 color = new Color32(0, 0, 0, alpha);

        switch (pcolor) {
            case PlayerColor.Black:
                color = new Color32(16, 16, 16, alpha);
                break;
            case PlayerColor.Red:
                color = new Color32(255, 46, 25, alpha);
                break;
            case PlayerColor.Green:
                color = new Color32(41, 218, 32, alpha);
                break;
            case PlayerColor.Blue:
                color = new Color32(0, 119, 255, alpha);
                break;
            case PlayerColor.Cyan:
                color = new Color32(73, 225, 255, alpha);
                break;
            case PlayerColor.Purple:
                color = new Color32(185, 33, 235, alpha);
                break;
            case PlayerColor.Magenta:
                color = new Color32(255, 91, 240, alpha);
                break;
            case PlayerColor.Orange:
                color = new Color32(255, 139, 0, alpha);
                break;
            case PlayerColor.Yellow:
                color = new Color32(255, 255, 76, alpha);
                break;
        }

        return color;
    }
}
