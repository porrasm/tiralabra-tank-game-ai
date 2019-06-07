using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerList : MonoBehaviour {

    private ClientManager manager;
    private GameObject listObject;
    private Color color1, color2;

    private struct PlayerObject {
        public Player player;
        public GameObject pObject;
    }

    private PlayerObject[] playerObjects;
    private GameObject playerListObject;

    private GameObject table;

    void Start() {
        manager = Scripts.GetScriptComponent<ClientManager>();
        playerObjects = new PlayerObject[8];

        for (int i = 0; i < 8; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void Update() {
        CheckUpdate();
    }

    public void CheckUpdate() {

        Player[] players = manager.GetPlayers();

        if (Input.GetKeyDown(KeyCode.Space)) {
            UpdateList(players);
            return;
        }

        if (NeedToUpdate(players)) {
            UpdateList(players);
        }
    }
    private void UpdateList(Player[] players) {

        print("Updating player list");

        for (int i = 0; i < 8; i++) {
            transform.GetChild(i).GetComponent<PlayerListObject>().SetInfo(players[i]);
        }
    }

    private void UpdateListObject(Transform t, Player c, int index) {

        t.gameObject.SetActive(true);

        // Name
        t.GetChild(0).GetComponent<Text>().text = c.Name;

        // Score
        t.GetChild(1).GetComponent<Text>().text = "" + c.Score;

        // Color
        //t.GetChild(1).GetChild(0).GetComponent<Image>().color = Colors.GetBasicColor(c.Color);

        // Backgroud color
        Image bg = t.GetComponent<Image>();
        Color32 color = Colors.GetBasicColor(c.Color, 128);
        bg.color = color;
        //if (useColor1) {
        //    bg.color = color1;
        //} else {
        //    bg.color = color2;
        //}
    }

    private bool NeedToUpdate(Player[] players) {

        for (int i = 0; i < 8; i++) {
            
            if (!transform.GetChild(i).GetComponent<PlayerListObject>().Matches(players[i])) {
                return true;
            }
        }

        return false;
    }
    private bool PlayerMatch(Player p, int index) {

        Transform listPlayer = transform.GetChild(index);

        return true;

    }

    public void AddPlayer(Player player) {

        PlayerObject playerObject = new PlayerObject();
        playerObject.player = player;

        GameObject newListObject = Instantiate(playerListObject);
        RectTransform rect = newListObject.GetComponent<RectTransform>();
        rect.SetParent(GetComponent<RectTransform>());

        Vector3 listPos = new Vector3(0, player.ID * rect.sizeDelta.y, 0);

        newListObject.GetComponent<RectTransform>().localPosition = listPos;

        playerObject.pObject = newListObject;

        RemovePlayer(player);
        playerObjects[player.ID] = playerObject;
    }
    public void RemovePlayer(Player player) {

        if (playerObjects[player.ID].player == null) {
            return;
        }

        Destroy(playerObjects[player.ID].pObject);

        playerObjects[player.ID] = new PlayerObject();
    }
}
