using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPlayer : TankPlayerBehavior {

    #region fields
    private Player owner;

    private int health;
    private int score;

    public enum PlayerState { Disabled, Locked, Enabled }
    #endregion

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
}
