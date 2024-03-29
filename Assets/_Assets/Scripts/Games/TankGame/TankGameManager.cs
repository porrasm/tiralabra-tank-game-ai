﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TankGameManager : MonoBehaviour {

    private const float InitializeWaitTime = 1f;

    private bool roundIsOn = false;

    private TankLevelGenerator generator;

    public delegate void RoundStart();
    private RoundStart roundCallbacks;

    private void Start() {
        generator = GameObject.FindGameObjectWithTag("Level").GetComponent<TankLevelGenerator>();
        roundIsOn = true;
        Invoke("Enable", InitializeWaitTime);
    }
    private void Enable() {
        roundIsOn = false;
    }

    private void Update() {
        StartRound();
    }

    private TankNetworking[] Players {
        get {
            return GameObject.FindGameObjectsWithTag("Player").Select(g => g.GetComponent<TankNetworking>()).ToArray();
        }
    }

    public void StartGame() {
    }

    private bool AllInitialized() {

        foreach (TankNetworking p in Players) {
            if (p.networkObject == null || !p.networkObject.NetworkReady) {
                return false;
            }
        }

        return true;
    }

    #region Round
    private void StartRound() {

        if (roundIsOn) {
            return;
        }

        if (!AllInitialized()) {
            return;
        }

        roundIsOn = true;

        print("Round started");

        StartCoroutine(RoundCoroutine());
    }
    private IEnumerator RoundCoroutine() {

        float roundTime = 0;

        generator.GenerateLevel();

        SetPlayerPositions();
        SetPlayerStates(TankPlayer.PlayerState.Locked);

        generator.BuildGeneratedLevel();

        while (generator.Building) {
            yield return null;
        }

        // Emergency stop
        SetPlayerPositions();

        yield return new WaitForSeconds(TankSettings.ExtraWaitTime);

        TankNetworking[] players = Players;

        SetPlayerStates(TankPlayer.PlayerState.Enabled);

        CallRoundStart();

        while (roundTime < TankSettings.RoundTime && AliveCount(players) > 1) {
            roundTime += Time.deltaTime;
            yield return null;
        }

        if (AliveCount(players) == 1) {
            GiveWin();
        }

        print("Round ended");
        yield return new WaitForSeconds(TankSettings.RoundEndWaitTime);
        roundIsOn = false;
    }

    private int AliveCount(TankNetworking[] players) {

        int count = 0;

        foreach (TankNetworking p in players) {
            if (p.State == TankPlayer.PlayerState.Enabled) {
                count++;
            }
        }

        return count;
    }
    private void GiveWin() {
        foreach (TankNetworking p in Players) {
            if (p.State == TankPlayer.PlayerState.Enabled) {
                Debug.LogWarning("Not implemented");
                p.TankScore += TankSettings.WinScore;
            }
        }
    }

    private void SetPlayerPositions() {

        Queue<Transform> spawns = RandomSpawns();

        StopTanks();

        foreach (TankNetworking p in Players) {

            Transform spawn = spawns.Dequeue();

            p.transform.position = spawn.position;
            p.transform.eulerAngles = spawn.eulerAngles;
        }
    }
    private void SetPlayerStates(TankPlayer.PlayerState state) {
        foreach (TankPlayer p in Players.Select(p => p.GetComponent<TankPlayer>()).ToArray()) {
            p.SetPlayerState(state);
        }
    }
    private void StopTanks() {
        foreach (TankController c in Players.Select(p => p.GetComponent<TankController>()).ToArray()) {
            c.StopTank();
        }
    }

    private Queue<Transform> RandomSpawns() {

        System.Random rnd = new System.Random();

        List<Transform> spawns = new List<Transform>();
        foreach (Transform spawn in GameObject.FindGameObjectWithTag("Respawn").transform) {
            spawns.Add(spawn);
        }

        Queue<Transform> rSpawns = new Queue<Transform>();

        for (int i = 0; i < 4; i++) {

            int r = rnd.Next(0, 4 - i);

            rSpawns.Enqueue(spawns[i]);
            spawns.RemoveAt(i);
        }

        for (int i = 0; i < spawns.Count; i++) {

            int r = rnd.Next(0, spawns.Count);

            rSpawns.Enqueue(spawns[i]);
            spawns.RemoveAt(i);
            i--;
        }

        return rSpawns;
    }
    #endregion

    #region Callbacks
    public void SubscribeRoundStart(RoundStart callback) {
        roundCallbacks += callback;
    }

    private void CallRoundStart() {
        if (roundCallbacks != null) {
            roundCallbacks();
        }
    }
    #endregion

    public static TankGameManager Instance() {
        return GameObject.FindGameObjectWithTag("Game").GetComponent<TankGameManager>();
    }
}
