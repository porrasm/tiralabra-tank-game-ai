using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControls : MonoBehaviour {

    #region fields
    private TankNetworking net;

    private bool uiControls;
    #endregion

    private void Start() {
        uiControls = true;
        net = GetComponent<TankNetworking>();
    }

    private void Update() {
        if (net.State != TankPlayer.PlayerState.Enabled) {
            return;
        }

        PCTestControls();
        GyroControls();
    }
    private void PCTestControls() {

        // PC CONTROLS FOR TESTING

        Vector2 movement = Vector2.zero;
        float rotation = 0;

        if (Input.GetKey(KeyCode.W)) {
            movement.y += 1;
        }
        if (Input.GetKey(KeyCode.S)) {
            movement.y -= 1;
        }
        if (Input.GetKey(KeyCode.D)) {
            movement.x += 1;
        }
        if (Input.GetKey(KeyCode.A)) {
            movement.x -= 1;
        }

        if (movement != Vector2.zero || rotation != 0) {
            uiControls = false;
        }

        if (uiControls) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            net.Fire++;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            net.Powerup++;
        }

        net.Movement = movement;
        net.Rotation = rotation;
    }
    private void GyroControls() {

        if (!uiControls) {
            return;
        }


        Vector2 movement = Vector2.zero;

        movement.x = Input.acceleration.x;
        movement.y = Input.acceleration.y;

        net.Movement = movement;
    }
    private Quaternion GyroToUnity(Quaternion q) {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    #region Control Methods
    public void Fire() {
        net.Fire++;
    }

    public void Powerup() {
        net.Powerup++;
    }
    #endregion
}
