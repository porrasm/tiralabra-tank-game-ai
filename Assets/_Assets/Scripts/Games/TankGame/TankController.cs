using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {

    [SerializeField]
    private float speed, rotateSpeed;

    private TankControls controls;

    private RectTransform rect;
    private Rigidbody2D rb;

    private void Start() {
        //if (!Server.Networker.IsServer) {
        //    Destroy(this);
        //    return;
        //}

        rect = GetComponent<RectTransform>();
        rb = GetComponent<Rigidbody2D>();
        controls = GetComponent<TankControls>();

    }
    private void Update() {
        MoveTank();
    }


    private void MoveTank() {

        Vector2 velocity = controls.Movement.y * rect.up * speed * Time.deltaTime;
        float rotation = -controls.Movement.x * rotateSpeed * Time.deltaTime;

        rb.MoveRotation(rb.rotation + rotation);
        rb.MovePosition(rb.position + velocity);
    }

}
