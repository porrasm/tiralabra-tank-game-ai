using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListObject : MonoBehaviour {

    private int ID;
    private string Name;
    private int Score;
    private PlayerColor Color;
    private bool isNull = true;

    public void SetInfo(Player player) {

        if (player != null) {
            ID = player.ID;
            Name = player.Name;
            Color = player.Color;
            Score = player.Score;
            isNull = false;
            ReDraw();
        } else {
            isNull = true;
            ReDraw();
        }
    }
    public void ReDraw() {
        gameObject.SetActive(!isNull);

        if (isNull) {
            return;
        }

        transform.GetChild(0).GetComponent<Text>().text = Name;
        
        transform.GetChild(1).GetComponent<Text>().text = "" + Score;

        Image bg = transform.GetComponent<Image>();
        Color32 color = Colors.GetBasicColor(Color, 128);
        bg.color = color;
    }
    public bool Matches(Player p) {

        if (p == null && isNull) {
            return true;
        } 

        if (p != null && !isNull) {
            bool names = Name.Equals(p.Name);
            bool ids = ID == p.ID;
            bool color = Color == p.Color;
            bool score = Score == p.Score;

            return names && ids && color && score;
        }

        return false;
    }
}
