using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPlayer : TankPlayerBehavior {

    #region fields
    private Player owner;

    private int health;
    private int score;

    private int roundWins;
    private int kills;
    private int assists;

    public PlayerState State { get; private set; }

    public enum PlayerState { Disabled = 0, Locked = 1, Enabled = 2 }
    #endregion

    protected override void NetworkStart() {
        base.NetworkStart();

        if (networkObject.IsServer) {
            transform.SetParent(GameObject.FindGameObjectWithTag("Game").transform);
            transform.localScale = new Vector3(1, 1, 1);
            owner = Player.MyPlayer(networkObject.MyPlayerId);
        }
    }

    private void Update() {
        UpdateVariables();
    }

    private void UpdateVariables() {

        if (networkObject.IsServer) {

            networkObject.Health = health;
            networkObject.Score = score;

            return;
        }

        health = networkObject.Health;
        score = networkObject.Score;
    }

    #region Game
    public void DoDamage(int damage, TankPlayer player) {

        health -= damage;

        if (health <= 0) {
            SetPlayerState(PlayerState.Disabled);
            player.KilledPlayer(this);
        }
    }

    public void KillPlayer() {
        SetPlayerState(PlayerState.Disabled);
    }
    public void SetPlayerState(PlayerState state) {

        if (!networkObject.IsServer) {
            return;
        }

        this.State = state;
        networkObject.SendRpc(RPC_CHANGE_STATE_R_P_C, Receivers.All, (byte)state);

        UpdateState();
    }
    private void UpdateState() {
        if (State == PlayerState.Enabled) {
            transform.GetChild(0).gameObject.SetActive(true);
            GetComponent<TankController>().enabled = true;
            SetAlive();
        }
        if (State == PlayerState.Locked) {
            transform.GetChild(0).gameObject.SetActive(true);
            GetComponent<TankController>().enabled = false;
        }
        if (State == PlayerState.Disabled) {
            transform.GetChild(0).gameObject.SetActive(false);
            GetComponent<TankController>().enabled = false;
            Kill();
        }
    }
    #endregion

    #region Events
    public void KilledPlayer(TankPlayer player) {

        if (Equals(player)) {
            player.AddScore(TankSettings.SuicideScore);
            player.kills--;
        } else {
            player.AddScore(TankSettings.KillScore);
            player.kills++;
        }
    }
    public void WinRound() {
        roundWins++;
        AddScore(TankSettings.WinScore);
    }
    #endregion

    private void SetAlive() {

        print("Setting health to: " + TankSettings.Health);

        health = TankSettings.Health;
    }
    private void Kill() {
        health = 0;
    }

    #region RPCS
    public override void ChangeStateRPC(RpcArgs args) {
        byte stateByte = args.GetNext<byte>();
        State = (PlayerState)stateByte;
    }
    #endregion

    public void AddScore(int score) {

        if (!networkObject.IsOwner) {
            return;
        }

        networkObject.Score += score;
        score += score;
    }

    public override bool Equals(object other) {

        TankPlayer p = other as TankPlayer;

        if (p == null) {
            return false;
        }

        return owner.ID == p.owner.ID;
    }
}
