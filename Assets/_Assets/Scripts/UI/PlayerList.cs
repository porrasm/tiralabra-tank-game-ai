using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerList : MonoBehaviour {

    #region fields
    private ClientManager manager;
    private GameObject listObject; 

    private PlayerObjectPair[] playerObjects;
    private GameObject playerListObject;

    private GameObject table;

    private struct PlayerObjectPair {
        public Player Player;
        public GameObject Object;
    }
    #endregion

    private void Start() {
        manager = Scripts.GetScriptComponent<ClientManager>();
        playerObjects = new PlayerObjectPair[8];

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

        for (int i = 0; i < 8; i++) {
            transform.GetChild(i).GetComponent<PlayerListObject>().SetInfo(players[i]);
        }
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

        PlayerObjectPair playerObject = new PlayerObjectPair();
        playerObject.Player = player;

        GameObject newListObject = Instantiate(playerListObject);
        RectTransform rect = newListObject.GetComponent<RectTransform>();
        rect.SetParent(GetComponent<RectTransform>());

        Vector3 listPos = new Vector3(0, player.ID * rect.sizeDelta.y, 0);

        newListObject.GetComponent<RectTransform>().localPosition = listPos;

        playerObject.Object = newListObject;

        RemovePlayer(player);
        playerObjects[player.ID] = playerObject;
    }
    public void RemovePlayer(Player player) {

        if (playerObjects[player.ID].Player == null) {
            return;
        }

        Destroy(playerObjects[player.ID].Object);

        playerObjects[player.ID] = new PlayerObjectPair();
    }
}
