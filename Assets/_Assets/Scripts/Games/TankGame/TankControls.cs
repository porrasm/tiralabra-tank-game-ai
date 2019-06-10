using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControls : TankControlsBehavior {

    #region fields
    private Vector2 movement;
    private float rotation;
    private float headRotation;
    private int fire;
    private int powerup;

    public Vector2 Movement { get => movement; set => movement = value; }
    public float Rotation { get => rotation; set => rotation = value; }
    public float HeadRotation { get => headRotation; set => headRotation = value; }
    public int Fire { get => fire; set => fire = value; }
    public int Powerup { get => powerup; set => powerup = value; }
    #endregion

    private Transform head;


    private void Start() {
        head = transform.GetChild(0);
    }

    private void Update() {
        UpdateControls();
        SendControls();
    }
    private void UpdateControls() {

        if (!networkObject.IsOwner) {
            return;
        }

        Movement = Vector2.zero;
        rotation = 0;

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
    }
    private void SendControls() {
        if (networkObject.IsServer) {

            movement = networkObject.Movement;
            rotation = networkObject.Rotation;
            headRotation = networkObject.HeadRotation;
            fire = networkObject.Fire;
            powerup = networkObject.Powerup;

            return;
        }

        networkObject.Movement = movement;
        networkObject.Rotation = rotation;
        networkObject.HeadRotation = headRotation;
        networkObject.Fire = fire;
        networkObject.Powerup = powerup;
    }
}
