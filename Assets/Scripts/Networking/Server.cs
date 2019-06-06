using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server {

    public static NetWorker networker { get; private set; }

    private static NetworkManager manager;

    static string ip = "127.0.0.1";
    public const ushort PORT = 15937;

    public static bool IsServer = false;

    public static bool StartServer() {

        networker = new TCPServer(64);
        MonoBehaviour.print("Hosting: " + ip + ":" + PORT);
        ((TCPServer)networker).Connect(ip, PORT);

        networker.playerTimeout += PlayerTimeout;
        //LobbyService.Instance.Initialize(server);

        bool created = Connected(networker);

        IsServer = created;

        if (created) {
            networker.playerAccepted += new NetWorker.PlayerEvent(OnPlayerJoin);
        }

        return created;
    }
    public static bool ConnectToServer() {

        MonoBehaviour.print("Joining game");

        networker = new TCPClient();
        MonoBehaviour.print("Joining: " + ip + ":" + PORT);
        ((TCPClient)networker).Connect(ip, PORT);

        bool connected = Connected(networker);

        if (connected) {
            networker.serverAccepted += new NetWorker.BaseNetworkEvent(OnServerConnect);
        }

        return connected;
    }

    private static bool Connected(NetWorker networker) {
        if (!networker.IsBound) {
            Debug.LogError("NetWorker failed to bind");
            return false;
        }

        GetManager();

        manager.Initialize(networker, "", PORT, null);

        NetworkObject.Flush(networker); //Called because we are already in the correct scene!
        return true;
    }
    private static void GetManager() {

        GameObject old = GameObject.FindGameObjectWithTag("NetworkManager");

        if (old) {
            if (old.GetComponent<NetworkManager>()) {
                manager = old.GetComponent<NetworkManager>();
                return;
            }
            Debug.LogError("Network manager not found");
            MonoBehaviour.Destroy(old);
        }

        GameObject managerObject = new GameObject();
        managerObject.name = "NetworkManager";
        managerObject.tag = "NetworkManager";
        manager = managerObject.AddComponent<NetworkManager>();
    }

    private static void PlayerTimeout(NetworkingPlayer player, NetWorker sender) {
        MonoBehaviour.print("Player " + player.NetworkId + " timed out");
    }

    // Other
    private static void OnPlayerJoin(NetworkingPlayer player, NetWorker sender) {
        MonoBehaviour.print("Player connected: " + player.NetworkId);

    }
    private static void OnServerConnect (NetWorker sender) {
        MonoBehaviour.print("Joined server");

    }
}
