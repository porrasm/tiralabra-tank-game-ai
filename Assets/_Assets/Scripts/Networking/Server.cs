using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server {

    #region fields
    public const ushort PORT = 15937;
    public static string IP = "127.0.0.1";

    private static NetworkManager manager;

    public static NetWorker Networker { get; private set; }
    #endregion

    public static bool StartServer() {

        SetIP();

        Networker = new TCPServer(64);
        ((TCPServer)Networker).Connect(IP, PORT);

        Networker.playerTimeout += PlayerTimeout;

        bool created = Connected(Networker);

        if (created) {
            Networker.playerAccepted += new NetWorker.PlayerEvent(OnPlayerJoin);
        }

        return created;
    }
    public static void SetIP() {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList) {
            if (ip.AddressFamily == AddressFamily.InterNetwork) {
                IP = ip.ToString();
                return;
            }
        }
    }

    public static bool ConnectToServer() {
        MonoBehaviour.print("Joining game");

        Networker = new TCPClient();
        MonoBehaviour.print("Joining: " + IP + ":" + PORT);
        ((TCPClient)Networker).Connect(IP, PORT);

        bool connected = Connected(Networker);

        if (connected) {
            Networker.serverAccepted += new NetWorker.BaseNetworkEvent(OnServerConnect);
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

        NetworkObject.Flush(networker);
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

    #region events
    private static void OnPlayerJoin(NetworkingPlayer player, NetWorker sender) {
        MonoBehaviour.print("Player connected: " + player.NetworkId);
    }
    private static void OnServerConnect(NetWorker sender) {
        MonoBehaviour.print("Joined server");
    }
    private static void PlayerTimeout(NetworkingPlayer player, NetWorker sender) {
        MonoBehaviour.print("Player " + player.NetworkId + " timed out");
    }
    #endregion
}
