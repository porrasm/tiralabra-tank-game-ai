using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
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
        }

        if (networkObject.IsOwner || Player.MyPlayer().Local) {
            SetOwner(Player.MyPlayer(), false);
        }

        GetComponent<TankPlayer>().enabled = true;
        GetComponent<TankControls>().enabled = true;
        GetComponent<TankWeapon>().enabled = true;
    }

    private void Update() {

        if (networkObject == null || Owner == null) {
            return;
        }

        UpdateFields();
    }

    public void UpdateFields() {

        if (networkObject.IsServer) {
            if (Owner.Local || Owner.AI) {
                ServerOwnerFieldUpdate();
            } else {
                ServerFieldUpdate();
            }
        } else if (networkObject.IsOwner) {
            OwnerFieldUpdate();
        } else {
            ClientFieldUpdate();
        }
    }
    private void ServerOwnerFieldUpdate() {
        networkObject.Health = health;
        networkObject.Score = tankScore;

        networkObject.Movement = movement;
        networkObject.Rotation = rotation;
        networkObject.Fire = fire;
        networkObject.Powerup = powerup;
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

    public void SetOwner(Player player, bool force) {

        if (networkObject == null) {
            print("Setting owner before net init");
        }
        if (player == null) {
            Debug.LogError("given player was null");
        }

        print("SetOwner: " + player.Name + ", " + force + ", " + player.Color);

        networkObject.SendRpc(RPC_SET_OWNER_R_P_C, Receivers.All, player.ID, force);
    }
    public override void SetOwnerRPC(RpcArgs args) {

        print("OWNER RPC -----------------------");

        int id = args.GetNext<int>();
        bool force = args.GetNext<bool>();

        if (owner != null && !force) {
            print("Owner not set");
            return;
        }

        owner = Player.PlayerByID(id);

        print("Owner set to " + owner.Name);

        if (networkObject.IsServer) {
            print("Setting color to " + owner.Color);
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
    public static void MyTank(Action<TankControls> callback) {

        IEnumerator SearchCoroutine() {
            GameObject tank;

            float time = 10;

            while (true) {
                yield return null;

                time -= Time.deltaTime;

                tank = MyTank();

                if (tank != null) {
                    break;
                }

                if (time < 0) {
                    print("Tank not found");
                    yield break;
                }
            }

            print("Tank found: " + tank.name);
            callback(tank.GetComponent<TankControls>());
        }

        Scripts.RunCoroutine(SearchCoroutine());
    }
    public static TankNetworking[] Tanks() {
        return GameObject.FindGameObjectsWithTag("Player").Select(o => o.GetComponent<TankNetworking>()).ToArray();
    }
}
