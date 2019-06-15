using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankNetworking : TankNetworkingBehavior {

    #region fields
    private Player owner;

    private int health;
    private int tankScore;
    private Vector2 movement;
    private float rotation;
    private int fire;
    private int powerup;

    public TankPlayer.PlayerState State { get; private set; }
    public Player Owner { get => owner; }
    public int Health { get => health; set => health = value; }
    public int TankScore { get => tankScore; set => tankScore = value; }
    public Vector2 Movement { get => movement; set => movement = value; }
    public float Rotation { get => rotation; set => rotation = value; }
    public int Fire { get => fire; set => fire = value; }
    public int Powerup { get => powerup; set => powerup = value; }

    private Vector2 Test() { return movement; }
    #endregion

    private void Awake() {
        State = TankPlayer.PlayerState.Disabled;
    }
    protected override void NetworkStart() {
        base.NetworkStart();

        networkObject.UpdateInterval = 31;

        if (!networkObject.IsServer && !networkObject.IsOwner) {
            return;
        }

        if (networkObject.IsServer) {
            GetComponent<TankController>().enabled = true;
        } else if (networkObject.IsOwner) {
            owner = Player.MyPlayer();
            networkObject.SendRpc(RPC_SET_OWNER_R_P_C, Receivers.ServerAndOwner, owner.ID);
        }

        GetComponent<TankPlayer>().enabled = true;
        GetComponent<TankControls>().enabled = true;
    }

    private void Update() {

        if (networkObject == null) {
            return;
        }

        UpdateFields();
    }

    public void UpdateFields() {
        if (networkObject.IsServer) {
            ServerFieldUpdate();
        } else if (networkObject.IsOwner) {
            OwnerFieldUpdate();
        } else {
            ClientFieldUpdate();
        }
    }
    private void ServerFieldUpdate() {
        networkObject.Health = health;
        networkObject.Score = tankScore;

        movement = networkObject.Movement;
        rotation = networkObject.Rotation;
        fire = networkObject.Fire;
        powerup = networkObject.Powerup;
    }
    private void OwnerFieldUpdate() {
        networkObject.Movement = movement;
        networkObject.Rotation = rotation;
        networkObject.Fire = fire;
        networkObject.Powerup = powerup;

        health = networkObject.Health;
        tankScore = networkObject.Score;
    }
    private void ClientFieldUpdate() {

        // Necessary?
        health = networkObject.Health;
        tankScore = networkObject.Score;
    }

    #region RPCs
    public void ChangeState(TankPlayer.PlayerState state) {
        State = state;
        networkObject.SendRpc(RPC_CHANGE_STATE_R_P_C, Receivers.All, (byte)state);
    }
    public override void ChangeStateRPC(RpcArgs args) {
        byte stateByte = args.GetNext<byte>();
        State = (TankPlayer.PlayerState)stateByte;
    }

    public override void SetOwnerRPC(RpcArgs args) {
        int id = args.GetNext<int>();
        owner = Player.PlayerByID(id);

        if (networkObject.IsServer) {
            GetComponent<TankController>().SetMaterial();
        }
    }
    #endregion
    public static GameObject MyTank() {

        Player myPlayer = Player.MyPlayer();

        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Player")) {

            TankNetworking n = o.GetComponent<TankNetworking>();
            if (n.Owner == null) {
                continue;
            }

            if (Player.Matches(n.Owner, myPlayer)) {
                return o;
            }
        }

        return null;
    }
}
