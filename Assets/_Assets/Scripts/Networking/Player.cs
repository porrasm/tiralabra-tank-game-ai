using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using System.Linq;
using UnityEngine;

public class Player : ClientBehavior {

    #region fields
    [SerializeField]
    private bool local;
    [SerializeField]
    private bool ai;

    private string playerName;
    private int score;

    private uint networkPlayerID = uint.MaxValue;
    public int ID { get; set; }
    public PlayerColor Color { get; set; }
    public string Name {
        get {
            if (playerName == null) {
                return "";
            } else {
                return playerName;
            }
        }
        set {
            playerName = value;
        }
    } 
    public int Score {
        get {
            if (networkObject.IsServer) {
                return score;
            } else {
                score = networkObject.Score;
                return score;
            }
        } set {
            if (networkObject.IsServer) {
                score = value;
                networkObject.Score = score;
            } 
        } }
    public byte Ready { get; set; }
    public bool Primary { get { return ID == 0; } }

    public NetworkingPlayer NetPlayer { get; set; }
    public bool Local { get => local; set {
            if (Server.Networker.IsServer) {
                local = value;
            } else {
                Debug.LogError("Setting local failed");
            }
        } }
    public bool AI { get => ai; set {
            if (Server.Networker.IsServer) {
                ai = value;
            }
        } }
    #endregion

    #region Initialization
    protected override void NetworkStart() {
        base.NetworkStart();

        InitializeClient();
    }

    private void Start() {
        transform.SetParent(GameObject.FindGameObjectWithTag("Players").transform);
    }
    
    public static void CreateNewClient() {
        NetworkManager.Instance.InstantiateClient();
    }
    public static void CreateNewAI() {
        ClientBehavior beh = NetworkManager.Instance.InstantiateClient();
    }
    public static void CreateNewLocalPlayer() {

    }

    public void InitializeClient() {

        networkPlayerID = networkObject.MyPlayerId;

        if (!Server.Networker.IsServer) {
            networkObject.SendRpc(RPC_SET_NETWORK_PLAYER_I_D_R_P_C, Receivers.All, networkObject.MyPlayerId);
            return;
        }

        Scripts.GetScriptComponent<ClientManager>().AddPlayer(this);

        UpdateClient();
    }
    #endregion

    public void UpdateClient() {
        if (!networkObject.IsOwner && !Server.Networker.IsServer) {
            print("Was not server or owner");
            return;
        }

        networkObject.SendRpc(RPC_UPDATE_CLIENT_RPC, Receivers.AllBuffered, (byte)ID, (byte)Color, Name, Ready);
    }
    public void ToggleColor() {
        networkObject.SendRpc(RPC_TOGGLE_COLOR_RPC, Receivers.AllBuffered);
    }
    public void ChangeName(string name) {
        if (!networkObject.IsOwner && !Server.Networker.IsServer) {
            print("Was not server or owner");
            return;
        }

        networkObject.SendRpc(RPC_CHANGE_NAME_RPC, Receivers.AllBuffered, name);
    }

    #region RPCs
    public override void UpdateClientRpc(RpcArgs args) {

        ID = args.GetAt<byte>(0);
        Color = (PlayerColor)args.GetAt<byte>(1);
        Name = args.GetAt<string>(2);
        Ready = args.GetAt<byte>(3);


        // If byte value is 2, the game will start
        if (networkObject.IsServer) {
            if (Ready == 2) {
                GameManager.StartGame();
            }
        }
    }
    public override void ToggleColorRpc(RpcArgs args) {

        if (!networkObject.IsServer) {
            return;
        }

        Color = Scripts.GetScriptComponent<ClientManager>().GetNextFreeColor(Color);
        UpdateClient();
    }
    public override void ChangeNameRpc(RpcArgs args) {

        if (!networkObject.IsServer) {
            return;
        }

        string name = args.GetAt<string>(0);

        Name = Scripts.GetScriptComponent<ClientManager>().GetFreeName(name);
        UpdateClient();
    }
    public override void SetNetworkPlayerIDRPC(RpcArgs args) {
        networkPlayerID = args.GetNext<uint>();
    }
    #endregion

    #region tools
    public static bool Matches(Player a, Player b) {

        if (a == null && b == null) {
            return true;
        } else if (a != null && b != null) {
            return a.Matches(b);
        }

        return false;
    }
    public bool Matches(Player other) {

        if (other == null) {
            return false;
        }

        bool names = Name.Equals(other.Name);
        bool ids = ID == other.ID;
        bool color = Color == other.Color;

        return names && ids && color;
    }

    public override string ToString() {
        return "Player " + ID + ", Name: " + Name + ", Color: " + Color;
    }

    public static Player MyPlayer() {
        GameObject parent = GameObject.FindGameObjectWithTag("Players");

        foreach (Transform child in parent.transform) {
            Player p = child.GetComponent<Player>();

            if (p.networkObject.IsOwner) {
                return p;
            } else if (p.local) {
                return p;
            }
        }

        return null;
    }
    public static Player MyPlayer(uint networkPlayerID) {
        GameObject parent = GameObject.FindGameObjectWithTag("Players");

        foreach (Transform child in parent.transform) {
            Player p = child.GetComponent<Player>();

            if (p.networkPlayerID == networkPlayerID) {
                return p;
            }
        }

        return null;
    }
    public static Player PlayerByID(int id) {
        GameObject parent = GameObject.FindGameObjectWithTag("Players");

        foreach (Transform child in parent.transform) {
            Player p = child.GetComponent<Player>();

            if (p.ID == id) {
                return p;
            }
        }

        return null;
    }
    #endregion
}
