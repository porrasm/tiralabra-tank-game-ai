using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControls : MonoBehaviour {

    public Vector2 Movement;
    public float Rotation;

    private Transform head;

    private void Start() {
        head = transform.GetChild(0);
    }

    private void Update() {
        UpdateControls();
    }
    private void UpdateControls() {

        Movement = Vector2.zero;
        Rotation = 0;

        if (Input.GetKey(KeyCode.W)) {
            Movement.y += 1;
        }
        if (Input.GetKey(KeyCode.S)) {
            Movement.y -= 1;
        }
        if (Input.GetKey(KeyCode.D)) {
            Movement.x += 1;
        }
        if (Input.GetKey(KeyCode.A)) {
            Movement.x -= 1;
        }

    }
}
