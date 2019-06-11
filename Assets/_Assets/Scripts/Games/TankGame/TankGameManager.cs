using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TankGameManager : MonoBehaviour {

    private bool roundIsOn = false;

    private TankLevelGenerator generator;

    private void Start() {
        generator = GameObject.FindGameObjectWithTag("Level").GetComponent<TankLevelGenerator>();
    }
    private void Update() {
        StartRound();
    }

    private TankPlayer[] Players {
        get {
            return GameObject.FindGameObjectsWithTag("Player").Select(g => g.GetComponent<TankPlayer>()).ToArray();
        }
    }

    public void StartGame() {

    }

    #region Round
    private void StartRound() {

        if (roundIsOn) {
            return;
        }

        roundIsOn = true;

        print("Round started");

        StartCoroutine(RoundCoroutine());
    }
    private IEnumerator RoundCoroutine() {

        float roundTime = 0;

        SetPlayerPositions();
        SetPlayerStates(TankPlayer.PlayerState.Locked);

        generator.GenerateLevel();

        while (generator.Generating) {
            yield return null;
        }

        yield return new WaitForSeconds(TankSettings.ExtraWaitTime);

        TankPlayer[] players = Players;

        SetPlayerStates(TankPlayer.PlayerState.Enabled);

        while (roundTime < TankSettings.RoundTime && AliveCount(players) >= 1) {
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
    private int AliveCount(TankPlayer[] players) {

        int count = 0;

        foreach (TankPlayer p in players) {
            if (p.State == TankPlayer.PlayerState.Enabled) {
                count++;
            }
        }

        print("Alive count: " + count);

        return count;
    }
    private void GiveWin() {
        foreach (TankPlayer p in Players) {
            if (p.State == TankPlayer.PlayerState.Enabled) {
                p.WinRound();
            }
        }
    }

    private void SetPlayerPositions() {

        Queue<Transform> spawns = RandomSpawns();

        foreach (TankPlayer p in Players) {

            Transform spawn = spawns.Dequeue();

            p.transform.position = spawn.position;
            p.transform.eulerAngles = spawn.eulerAngles;
        }
    }
    private void SetPlayerStates(TankPlayer.PlayerState state) {
        foreach (TankPlayer p in Players) {
            p.SetPlayerState(state);
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
        }

        return rSpawns;
    }
    #endregion


}
