using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colors {

    public static Color GetBasicColor(PlayerColor pColor) {

        Color32 color = new Color32(0, 0, 0, 255);

        switch (pColor) {
            case PlayerColor.Black:
                color = new Color32(16, 16, 16, 255);
                break;
            case PlayerColor.Red:
                color = new Color32(255, 46, 25, 255);
                break;
            case PlayerColor.Green:
                color = new Color32(41, 218, 32, 255);
                break;
            case PlayerColor.Blue:
                color = new Color32(0, 119, 255, 255);
                break;
            case PlayerColor.Cyan:
                color = new Color32(73, 225, 255, 255);
                break;
            case PlayerColor.Purple:
                color = new Color32(185, 33, 235, 255);
                break;
            case PlayerColor.Magenta:
                color = new Color32(255, 91, 240, 255);
                break;
            case PlayerColor.Orange:
                color = new Color32(255, 139, 0, 255);
                break;
            case PlayerColor.Yellow:
                color = new Color32(255, 255, 76, 255);
                break;
        }

        return color;
    }

}
