using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : ClientBehavior {

    public int ID { get; set; }
    public PlayerColor Color { get; set; }
    public string Name { get; set; }
    public NetworkingPlayer nPlayer { get; set; }

    public static void CreateNewClient() {
        print("Creating new client object");
        NetworkManager.Instance.InstantiateClient();
    }

    private void Start() {
        transform.SetParent(GameObject.FindGameObjectWithTag("Players").transform);
        InitializeClient();
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
            return;
        }

        print("Sending client info over RPC");
        networkObject.SendRpc(RPC_UPDATE_CLIENT_RPC, Receivers.AllBuffered, (byte)ID, (byte)Color, Name);
    }

    #region RPCs
    public override void UpdateClientRpc(RpcArgs args) {

        print("Received RPC call");

        ID = args.GetAt<byte>(0);
        Color = (PlayerColor)args.GetAt<byte>(1);
        Name = args.GetAt<string>(2);

        string objectName = "Player " + ID + ": " + Name;

        print("Updating client: " + Name);
        print("Setting gameobject name to: " + objectName);
        gameObject.name = objectName;
    }
    #endregion
}
