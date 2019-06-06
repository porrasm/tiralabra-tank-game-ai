using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour {

    private struct PlayerObject {
        public Client player;
        public GameObject pObject;
    }

    private PlayerObject[] playerObjects;
    private GameObject playerListObject;

    private GameObject table;

    void Start() {
        playerObjects = new PlayerObject[8];
        playerListObject = Resources.Load<GameObject>("ResourcePrefabs/PlayerListObject");

        string[,] content = new string[2,5];

        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 5; j++) {
                content[i, j] = "Row " + j + ", Column " + i;
            }
        }

        table = PanelTable.CreateTable(transform, content, new Vector3(100, 30, 1), true);
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
