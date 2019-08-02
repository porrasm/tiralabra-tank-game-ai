using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : GamesBehavior {

    private static Game currentGame;

    public enum Game { Menu = -2, Lobby = -1, TankGame = 1 }

    private void Start() {

        transform.SetParent(Scripts.GetGameObject().transform);

        currentGame = Game.Menu;
    }
    protected override void NetworkStart() {
        base.NetworkStart();
    }

    public static void InitGames() {

        string start = null;
        if (Server.Networker.IsServer) {
            start = "Host_";
        } else {
            Player.CreateNewClient();
            start = "Client_";
        }

        SceneManager.LoadScene(start + "Lobby");
    }

    public static void StartGame(Game game) {
        GameManager g = Scripts.GetScriptComponent<GameManager>();

        if (g != null) {
            g.SetSceneComp(game);
        } else {
            print("GameManager was null");
        }
    }
    public static void StartGame() {
        StartGame(Game.TankGame);
    }

    private void SetSceneComp(Game game) {

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

        if (currentGame == Game.Lobby) {

            SceneManager.LoadScene(start + "Lobby");
        } else if (currentGame == Game.TankGame) {

            SceneManager.LoadScene(start + "TankGame");
        }
    }


    #region RPCs
    public override void StartGameRpc(RpcArgs args) {

        int game = args.GetNext<int>();

        currentGame = (Game)game;
        LoadGameScene();
    }
    #endregion
}
