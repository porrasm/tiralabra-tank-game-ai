using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {


    private List<PlayerColor> takenColors;

    private void Start() {
        takenColors = new List<PlayerColor>();
    }

    public PlayerColor GetNextFreeColor(PlayerColor oldColor) {

        takenColors.Remove(oldColor);

        for (int plusAmount = 1; plusAmount < 9; plusAmount++) {

            PlayerColor newColor = (PlayerColor)((int)(oldColor + 1) % 8);

            if (IsFreeColor(newColor)) {
                return TakeColor(newColor);
            }
        }

        return PlayerColor.Black;
    }
    private bool IsFreeColor(PlayerColor color) {
        return !takenColors.Contains(color);
    }

    private PlayerColor TakeColor(PlayerColor color) {
        takenColors.Add(color);
        return color;
    }
}

public enum PlayerColor {
    Red = 0, Green, Blue, Cyan, Yellow, Orange, Purple, Magenta, Black
}