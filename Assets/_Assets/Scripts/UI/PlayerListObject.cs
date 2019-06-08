using UnityEngine;
using UnityEngine.UI;

public class PlayerListObject : MonoBehaviour {

    #region fields
    private int id;
    private string playerName;
    private int score;
    private PlayerColor color;
    private bool isNull = true;
    #endregion

    public void SetInfo(Player player) {

        if (player != null) {
            id = player.ID;
            playerName = player.Name;
            color = player.Color;
            score = player.Score;
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

        transform.GetChild(0).GetComponent<Text>().text = playerName;
        
        transform.GetChild(1).GetComponent<Text>().text = "" + score;

        Image bg = transform.GetComponent<Image>();
        Color32 bgColor = Colors.GetBasicColor(color, 128);
        bg.color = bgColor;
    }
    public bool Matches(Player p) {

        if (p == null && isNull) {
            return true;
        } 

        if (p != null && !isNull) {
            bool names = playerName.Equals(p.Name);
            bool ids = id == p.ID;
            bool color = this.color == p.Color;
            bool score = this.score == p.Score;

            return names && ids && color && score;
        }

        return false;
    }
}
