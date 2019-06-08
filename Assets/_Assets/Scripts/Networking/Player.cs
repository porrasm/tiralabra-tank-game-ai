using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

public class Player : ClientBehavior {

    #region fields
    private string playerName;
    private int score;

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
    public bool Ready { get; set; }
    public bool Primary { get { return ID == 0; } }

    public NetworkingPlayer NetPlayer { get; set; }
    #endregion

    protected override void NetworkStart() {
        base.NetworkStart();

        InitializeClient();
    }

    private void Start() {
        transform.SetParent(GameObject.FindGameObjectWithTag("Players").transform);
    }

    public static void CreateNewClient() {
        print("Creating new client object");
        NetworkManager.Instance.InstantiateClient();
    }

    public void InitializeClient() {
        if (!Server.Networker.IsServer) {
            return;
        }

        print("Initializing client");

        Scripts.GetScriptComponent<ClientManager>().AddPlayer(this);

        print("Initialized to " + Name);

        UpdateClient();
    }
    public void UpdateClient() {
        if (!networkObject.IsOwner && !Server.Networker.IsServer) {
            print("Was not server or owner");
            return;
        }

        byte ready = 0;
        if (Ready) {
            ready = 1;
        }

        print("Sending client info over RPC: " + Name);
        networkObject.SendRpc(RPC_UPDATE_CLIENT_RPC, Receivers.AllBuffered, (byte)ID, (byte)Color, Name, ready);
    }
    public void ToggleColor() {
        print("Toggling player color");
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

        print("Received RPC to update client");

        ID = args.GetAt<byte>(0);
        Color = (PlayerColor)args.GetAt<byte>(1);
        Name = args.GetAt<string>(2);
        Ready = args.GetAt<byte>(3) == 1;
    }
    public override void ToggleColorRpc(RpcArgs args) {

        if (!networkObject.IsServer) {
            print("TOGGLE COLOR: not server");
            return;
        }

        print("Toggling player color");
        Color = Scripts.GetScriptComponent<ClientManager>().GetNextFreeColor(Color);
        print("New color is: " + Color);
        UpdateClient();
    }
    public override void ChangeNameRpc(RpcArgs args) {

        if (!networkObject.IsServer) {
            return;
        }

        string name = args.GetAt<string>(0);

        if (name == null) {
            print("Name was null");
        }

        Name = Scripts.GetScriptComponent<ClientManager>().GetFreeName(name);
        UpdateClient();
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
            }
        }

        return null;
    }
    #endregion
}
