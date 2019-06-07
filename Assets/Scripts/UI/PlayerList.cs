using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerList : MonoBehaviour {

    private ClientManager manager;
    private GameObject listObject;
    private Color color1, color2;

    private Client[] players;

    private struct PlayerObject {
        public Client player;
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

        if (Input.GetKeyDown(KeyCode.Space)) {
            players = manager.GetPlayers();
            UpdateList();
            return;
        }

        Client[] newPlayers = manager.GetPlayers();

        if (NeedToUpdate(newPlayers)) {
            UpdateList();
        }
    }
    private void UpdateList() {

        print("Updating player list");

        int i = 0;

        foreach (Client c in players) {

            if (c == null) {
                continue;
            }

            UpdateListObject(transform.GetChild(i), c, i);

            i++;
        }

        for (;i < 8; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void UpdateListObject(Transform t, Client c, int index) {

        t.gameObject.SetActive(true);

        // Name
        t.GetChild(0).GetComponent<Text>().text = c.Name;
        // Color
        t.GetChild(1).GetChild(0).GetComponent<Image>().color = Colors.GetBasicColor(c.Color);

        // Backgroud color
        //Image bg = t.GetComponent<Image>();
        //if (useColor1) {
        //    bg.color = color1;
        //} else {
        //    bg.color = color2;
        //}
    }

    private bool NeedToUpdate(Client[] newPlayers) {

        if (players == null) {
            players = newPlayers;
            return true;
        }

        bool update = false;

        for (int i = 0; i < newPlayers.Length; i++) {
            if (!Client.Matches(players[i], newPlayers[i])) {

            }
        }

        if (update) {
            players = newPlayers;
        }
        return update;
    }

    public void AddPlayer(Client player) {

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
    public void RemovePlayer(Client player) {

        if (playerObjects[player.ID].player == null) {
            return;
        }

        Destroy(playerObjects[player.ID].pObject);

        playerObjects[player.ID] = new PlayerObject();
    }
}
