using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Games : GamesBehavior {

    private GameList currentGame;

    public enum GameList { Menu = -2, Lobby = -1, TankGame = 1 }

    private void Start() {

        transform.SetParent(Scripts.GetGameObject().transform);

        currentGame = GameList.Menu;
    }
    protected override void NetworkStart() {
        base.NetworkStart();

        print("Network start");
    }

    public static void StartGame() {

        string start = null;
        if (Server.Networker.IsServer) {
            start = "Host_";
        } else {
            Player.CreateNewClient();
            start = "Client_";
        }

        SceneManager.LoadScene(start + "Lobby");
    }

    public static void SetScene(GameList game) {
        Games g = Scripts.GetScriptComponent<Games>();

        if (g != null) {
            g.SetSceneComp(game);
        }
    }
    private void SetSceneComp(GameList game) {

        if (networkObject.IsServer) {
            networkObject.SendRpc(RPC_START_GAME_RPC, Receivers.All, (int)game);
        }
    }

    public void LoadGameScene() {

        string start = null;
        if (networkObject.IsServer) {
            start = "Host_";
        } else {
            start = "Client_";
        }

        if (currentGame == GameList.Lobby) {

            SceneManager.LoadScene(start + "Lobby");
        } else if (currentGame == GameList.TankGame) {

            SceneManager.LoadScene(start + "TankGame");
        }
    }


    #region RPCs
    public override void StartGameRpc(RpcArgs args) {

        int game = args.GetNext<int>();

        currentGame = (GameList)game;
        LoadGameScene();
    }
    #endregion
}
