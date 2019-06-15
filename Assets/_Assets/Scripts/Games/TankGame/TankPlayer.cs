﻿using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPlayer : MonoBehaviour {

    #region fields
    private TankNetworking net;

    private int roundWins;
    private int kills;
    private int assists;
     
    public enum PlayerState { Disabled = 0, Locked = 1, Enabled = 2 }
    #endregion

    private void Start() {
        net = GetComponent<TankNetworking>();
    }

    #region Game
    public void DoDamage(int damage, TankPlayer player) {

        net.Health -= damage;

        if (net.Health <= 0) {
            SetPlayerState(PlayerState.Disabled);
            player.KilledPlayer(this);
        }
    }

    public void KillPlayer() {
        SetPlayerState(PlayerState.Disabled);
    }
    public void SetPlayerState(PlayerState state) {

        if (!net.networkObject.IsServer) {
            return;
        }

        GetComponent<TankController>().StopTank();

        net.ChangeState(state);

        UpdateState();
    }
    private void UpdateState() {

        print("Player update state: " + net.State);

        if (net.State == PlayerState.Enabled) {
            transform.GetChild(0).gameObject.SetActive(true);
            SetAlive();
        }
        if (net.State == PlayerState.Locked) {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        if (net.State == PlayerState.Disabled) {
            transform.GetChild(0).gameObject.SetActive(false);
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

        net.Health = TankSettings.Health;
    }
    private void Kill() {
        net.Health = 0;
    }

    public void AddScore(int score) {
        net.TankScore += score;
    }

    public override bool Equals(object other) {

        TankPlayer p = other as TankPlayer;

        if (p == null) {
            return false;
        }

        return net.Owner.ID == p.net.Owner.ID;
    }  
}
