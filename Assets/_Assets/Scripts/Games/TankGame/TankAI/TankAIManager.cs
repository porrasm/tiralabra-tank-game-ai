﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is used for initializing the AI testing environment without the need to open 2 separate applications (server & client) at the same time.
/// The code contains some budget solutions just to get the environment up and running.
/// </summary>
public class TankAIManager : MonoBehaviour {

    public static int AICount;
    public static bool PlayerEnabled;

    #region fields
    #endregion

    private void Start() {
        InitializeTesting();
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.LeftShift)) {

            TankSettings.Debugging = !TankSettings.Debugging;

        }
        if (Input.GetKeyDown(KeyCode.Escape)) {

            Application.Quit();

            Server.StopServer();
            SceneManager.LoadScene(0);

            foreach (TankNetworking g in TankNetworking.Tanks()) {
                Destroy(g.gameObject);
                Destroy(g.Owner.gameObject);
            }
        }
    }

    private void InitializeTesting() {
        InitializeTestPlayer();
        InitializeAIs();
    }

    /// <summary>
    /// Spawns the player tank.
    /// </summary>
    private void InitializeTestPlayer() {
        NetworkManager.Instance.InstantiateTankNetworking();
    }

    /// <summary>
    /// Spawns ais and initializes all the players.
    /// </summary>
    private void InitializeAIs() {
        IEnumerator AIInit() {

            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < AICount; i++) {
                NetworkManager.Instance.InstantiateTankNetworking();
            }

            Player[] players = GameObject.FindGameObjectsWithTag("Client").Select(g => g.GetComponent<Player>()).ToArray();

            int ai = 1;

            TankNetworking[] tanks = GameObject.FindGameObjectsWithTag("Player").Select(t => t.GetComponent<TankNetworking>()).ToArray();

            for (int i = 0; i < players.Length; i++) {

                tanks[i].SetOwner(players[i], true);

                if (players[i].Local) {
                    players[i].ChangeName("Local player");
                } else {
                    tanks[i].gameObject.AddComponent<TankAI>();
                    players[i].AI = true;
                    players[i].ChangeName("AI " + ai);
                    ai++;
                }

                yield return null;

                // yield return new WaitForSeconds(0.2f);
            }
        }

        StartCoroutine(AIInit());
    }

    /// <summary>
    /// Sets the first players status to local to indicate that it is the one controller by the player.
    /// </summary>
    public static void ClaimLocalPlayer() {

        GameObject pObject = GameObject.FindGameObjectWithTag("Client");

        if (pObject == null) {
            Debug.LogError("No player was found.");
            return;
        }

        pObject.GetComponent<Player>().Local = true;
    }
}
