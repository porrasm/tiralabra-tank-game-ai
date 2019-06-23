using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public enum PlayerColor {
    Black = -1, Red = 0, Green = 1, Blue = 2, Cyan = 3, Yellow = 4, Orange = 5, Purple = 6, Magenta = 7
}

public class ClientManager : MonoBehaviour {

    #region fields
    private List<PlayerColor> takenColors;
    private Player[] players;

    private GameObject playerPrefab;
    #endregion

    private void Start() {
        takenColors = new List<PlayerColor>();
        players = new Player[8];
        playerPrefab = Resources.Load<GameObject>("ResourcePrefabs/Player");
    }

    public string GetFreeName(string name) {
        return GetFreeName(name, 0);
    }
    private string GetFreeName(string nameParam, int index) {

        string name = null;
        if (index == 0) {
            name = nameParam;
        } else {
            name = nameParam + index;
        }

        foreach (Player p in players) {

            if (p == null) {
                continue;
            }

            if (p.Name.Equals(name)) {
                return GetFreeName(nameParam, index + 1);
            }
        }

        // Fix input
        name = Regex.Replace(name.Trim(), @"[^a-zA-Z0-9\söäåÖÄÅ(:)]", string.Empty);
        name = Regex.Replace(name.Trim(), @"\s+", " ");

        return name;
    }

    #region Client Side
    public static bool AllReady() {

        foreach (Transform child in GameObject.FindGameObjectWithTag("Players").transform) {

            Player p = child.GetComponent<Player>();

            if (!p.Ready) {
                return false;
            }
        }

        return true;
    }
    #endregion

    #region Player Initialization
    public int GetFreeID() {
        for (int i = 0; i < 8; i++) {
            if (players[i] == null) {
                return i;
            }
        }

        return -1;
    }
    public void AddPlayer(Player client) {
        NewClientInfo(client, GetFreeID());
        players[client.ID] = client;
    }

    public Player[] GetPlayers() {
        return players;
    }

    public bool CreateNewPlayer(int id) {

        if (players[id] != null) {
            return false;
        }

        GameObject clientObject = Instantiate(playerPrefab);
        Player client = clientObject.GetComponent<Player>();
        

        players[id] = client;

        return true;
    }
    public void NewClientInfo(Player client, int id) {
        client.ID = id;
        client.Name = "Player " + (id + 1);
        client.Color = GetNextFreeColor(PlayerColor.Black);
    }

    #endregion

    #region Colors
    public PlayerColor GetNextFreeColor(PlayerColor oldColor) {

        Debug.Log("Getting next free color after: " + oldColor);

        for (int i = 0; i < 8; i++) {

            PlayerColor newColor = (PlayerColor)((int)(oldColor + i) % 8);

            if (IsFreeColor(newColor)) {
                takenColors.Remove(oldColor);
                return TakeColor(newColor);
            }
        }

        takenColors.Remove(oldColor);
        return PlayerColor.Black;
    }
    private bool IsFreeColor(PlayerColor color) {
        if (color == PlayerColor.Black) {
            return false;
        }
        return !takenColors.Contains(color);
    }
    private PlayerColor TakeColor(PlayerColor color) {
        takenColors.Add(color);
        return color;
    }
    #endregion
}