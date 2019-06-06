using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour {

    private List<PlayerColor> takenColors;
    private Client[] players;

    private GameObject playerPrefab;

    private void Start() {
        takenColors = new List<PlayerColor>();
        players = new Client[8];
        playerPrefab = Resources.Load<GameObject>("ResourcePrefabs/Player");
    }

    #region Player Initialization
    public int GetFreeID() {
        for (int i = 0; i < 8; i++) {
            if (players[i] == null) {
                return i;
            }
        }

        return -1;
    }
    public bool CreateNewPlayer(int id) {

        if (players[id] != null) {
            return false;
        }

        GameObject clientObject = Instantiate(playerPrefab);
        Client client = clientObject.GetComponent<Client>();
        

        players[id] = client;

        return true;
    }
    public void NewClientInfo(ref Client client, int id) {
        client.ID = id;
        client.Name = "Player " + id;
        client.Color = GetNextFreeColor(0);
    }

    #endregion

    #region Colors
    public PlayerColor GetNextFreeColor(PlayerColor oldColor) {

        if (IsFreeColor(oldColor)) {
            return TakeColor(oldColor);
        }

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
    #endregion
}

public enum PlayerColor {
    Red = 0, Green, Blue, Cyan, Yellow, Orange, Purple, Magenta, Black
}