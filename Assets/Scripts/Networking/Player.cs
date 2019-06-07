using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ClientBehavior {

    public int ID { get; set; }
    public PlayerColor Color { get; set; }
    private string name;
    public string Name { get { if (name == null) { return ""; } else { return name; } } set { name = value; } }
    public int Score { get; private set; }
    public NetworkingPlayer nPlayer { get; set; }

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
        if (!Server.networker.IsServer) {
            return;
        }

        print("Initializing client");

        Scripts.GetScriptComponent<ClientManager>().AddPlayer(this);

        print("Initialized to " + Name);

        UpdateClient();
    }
    public void UpdateClient() {
        if (!networkObject.IsOwner && !Server.networker.IsServer) {
            print("Was not server or owner");
            return;
        }

        print("Sending client info over RPC: " + Name);
        networkObject.SendRpc(RPC_UPDATE_CLIENT_RPC, Receivers.AllBuffered, (byte)ID, (byte)Color, Name);
    }
    public void ToggleColor() {
        print("Toggling player color");
        networkObject.SendRpc(RPC_TOGGLE_COLOR_RPC, Receivers.AllBuffered);
    }

    #region RPCs
    public override void UpdateClientRpc(RpcArgs args) {

        print("Received RPC to update client");

        ID = args.GetAt<byte>(0);
        Color = (PlayerColor)args.GetAt<byte>(1);
        Name = args.GetAt<string>(2);
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

        //MainThreadManager.Run(() => {
        //    print("Toggling player color");
        //    Color = Scripts.GetScriptComponent<ClientManager>().GetNextFreeColor(Color);
        //    print("New color is: " + Color);
        //    UpdateClient();
        //});

        Debug.Log("New color is: " + Color);
        //UpdateClient();
    }
    #endregion

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


}
