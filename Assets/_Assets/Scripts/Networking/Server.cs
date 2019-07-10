//Backup
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server {

    #region fields
    public const ushort PORT = 15937;
    public static string IP = "127.0.0.1";

    public static string natServerHost = string.Empty;
    public static ushort natServerPort = 15938;

    private static NetworkManager manager;

    public static NetWorker Networker { get; private set; }

    public static bool useTCP = false;
    #endregion

    public static void Initialize() {
        Rpc.MainThreadRunner = MainThreadManager.Instance;

        if (!useTCP) {
            // Do any firewall opening requests on the operating system
            NetWorker.PingForFirewall(PORT);
        }
    }

    public static bool StartServer() {

        SetIP();

        if (useTCP) {
            Networker = new TCPServer(64);
            ((TCPServer)Networker).Connect(IP, PORT);
        } else {
            Networker = new UDPServer(64);

            if (natServerHost.Trim().Length == 0) {
                ((UDPServer)Networker).Connect(IP, PORT);
            } else {
                ((UDPServer)Networker).Connect(port: PORT, natHost: natServerHost, natPort: natServerPort);
            }
        }

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

        //NetWorker.localServerLocated += LocalServerLocated;
        //NetWorker.RefreshLocalUdpListings(PORT);

        MonoBehaviour.print("Connecting to :" + IP);

        return ConnectToAddress(IP, PORT);
    }

    private static bool ConnectToAddress(string IP, ushort PORT) {
        MonoBehaviour.print("Joining game");

        //
        if (useTCP) {
            Networker = new TCPClient();
            MonoBehaviour.print("Joining: " + IP + ":" + PORT);
            ((TCPClient)Networker).Connect(IP, PORT);
        } else {
            Networker = new UDPClient();
            if (natServerHost.Trim().Length == 0) {
                ((UDPClient)Networker).Connect(IP, PORT);
            } else {
                ((UDPClient)Networker).Connect(IP, PORT, natServerHost, natServerPort);
            }
        }
        //

        bool connected = Connected(Networker);

        MonoBehaviour.print("Connected: " + connected);
        MonoBehaviour.print("Client: " + Networker);

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
