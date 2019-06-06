using BeardedManStudios.Forge.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server {

    private static TCPServer server;
    private static NetWorker client;

    private static CustomNetworkManager manager;

    static string ip = "127.0.0.1";
    public const ushort PORT = 15937;

    public static bool StartServer() {

        server = new TCPServer(64);
        MonoBehaviour.print("Hosting: " + ip + ":" + PORT);
        server.Connect(ip, PORT);

        server.playerTimeout += PlayerTimeout;
        //LobbyService.Instance.Initialize(server);

        return Connected(server);
    }
    public static bool ConnectToServer() {

        MonoBehaviour.print("Joining game");

        client = new TCPClient();
        MonoBehaviour.print("Joining: " + ip + ":" + PORT);
        ((TCPClient)client).Connect(ip, PORT);

        return Connected(client);
    }

    private static bool Connected(NetWorker networker) {
        if (!networker.IsBound) {
            Debug.LogError("NetWorker failed to bind");
            return false;
        }

        CreateManager();

        manager.Initialize(networker, "", PORT, null);

        NetworkObject.Flush(networker); //Called because we are already in the correct scene!
        return true;
    }
    private static void CreateManager() {

        GameObject old = GameObject.FindGameObjectWithTag("NetworkManager");

        if (old) {
            MonoBehaviour.Destroy(old);
        }

        GameObject managerObject = new GameObject();
        managerObject.name = "NetworkManager";
        managerObject.tag = "NetworkManager";
        manager = managerObject.AddComponent<CustomNetworkManager>();
    }

    private static void PlayerTimeout(NetworkingPlayer player, NetWorker sender) {
        MonoBehaviour.print("Player " + player.NetworkId + " timed out");
    }

}
