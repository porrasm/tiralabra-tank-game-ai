using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour {

    private struct PlayerObject {
        public Player player;
        public GameObject pObject;
    }

    private PlayerObject[] playerObjects;
    private GameObject playerListObject;

    void Start() {
        playerObjects = new PlayerObject[8];
        playerListObject = Resources.Load<GameObject>("ResourcePrefabs/PlayerListObject");
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
