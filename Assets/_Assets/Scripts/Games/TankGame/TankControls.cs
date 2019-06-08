using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControls : MonoBehaviour {

    public Vector2 Movement;
    public float Rotation;

    [SerializeField]
    private float speed, rotateSpeed;

    private Transform head;

    private void Start() {
        head = transform.GetChild(0);
    }

    private void Update() {
        UpdateControls();
        MoveTank();
    }

    private void MoveTank() {

        Vector3 move = new Vector3(0, 0, Movement.y) * speed * Time.deltaTime;
        float rotation = Movement.x * rotateSpeed * Time.deltaTime;

        // Rotate
        transform.Rotate(0, rotation, 0);
        head.Rotate(0, -rotation, 0);

        // Move
        transform.Translate(move);
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
