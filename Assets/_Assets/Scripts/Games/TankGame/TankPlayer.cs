﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TankPlayer : MonoBehaviour {

    #region fields
    private TankNetworking net;

    private int roundWins;
    private int kills;
    private int assists;

    private float healthDegen;

    // Switch with common class for special settings
    public bool Invulnerable { get; set; }

    public enum PlayerState {
        Disabled = 0, Locked = 1, Enabled = 2
    }
    #endregion

    private void Start() {
        net = GetComponent<TankNetworking>();
        net.Health = TankSettings.Health;
    }
    private void Update() {
        UpdateHealth();
    }

    #region Game
    public void DoDamage(int damage, TankPlayer player) {

        int start = net.Health;

        net.Health -= damage;

        if (net.Health > start) {
            Debug.Log("GAINED HEALTH: " + damage + ", p: " + player.net.Owner.name);
        }

        if (net.Health <= 0) {
            SetPlayerState(PlayerState.Disabled);
            player.KilledPlayer(this);
        }
    }

    public void KillPlayer() {
        SetPlayerState(PlayerState.Disabled);
    }
    public void SetPlayerState(PlayerState state) {

        if (net == null) {
            print("net was null");
        }
        if (net.networkObject == null) {
            print("net object was null");
        }

        if (!net.networkObject.IsServer) {
            return;
        }

        GetComponent<TankController>().StopTank();

        net.ChangeState(state);

        UpdateState();
    }
    private void UpdateState() {

        if (net.State == PlayerState.Enabled) {
            EnableChildren(true);
            SetAlive();
        }
        if (net.State == PlayerState.Locked) {
            EnableChildren(true);
        }
        if (net.State == PlayerState.Disabled) {
            EnableChildren(false);
            Kill();
        }
    }
    private void EnableChildren(bool enable) {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(enable);
        }
    }

    private void UpdateHealth() {

        // DO NOT LOWER HEALTH WHEN REGENERATING
        if (net.State != PlayerState.Enabled) {
            return;
        }

        if (net.Health > TankSettings.MaxHealth) {
            net.Health = TankSettings.MaxHealth;
        }

        if (net.Health >= TankSettings.Health) {
            healthDegen += Time.deltaTime * TankSettings.HealthLossRatePerSecond;
        }

        if (healthDegen > 1) {
            net.Health -= (int)healthDegen;
            healthDegen -= (int)healthDegen;
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

    public override int GetHashCode() {
        return gameObject.GetHashCode();
    }
}
