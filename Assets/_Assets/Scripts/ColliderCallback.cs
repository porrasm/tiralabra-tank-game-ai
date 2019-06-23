using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCallback : MonoBehaviour {

    public delegate void CollisionCallback(Collision collision);
    public delegate void TriggerCallback(Collider collider);
    private CollisionCallback collisions;
    private TriggerCallback tEnter;
    private TriggerCallback tExit;

    public void AddCollisionCallback(CollisionCallback callback) {
        collisions += callback;
    }

    public void AddTriggerEnterCallback(TriggerCallback callback) {
        tEnter += callback;
    }

    public void AddTriggerExitCallback(TriggerCallback callback) {
        tExit += callback;
    }

    private void OnCollisionEnter(Collision collision) {

        if (collisions == null) {
            return;
        }

        collisions(collision);
    }
    private void OnTriggerEnter(Collider collider) {

        if (tEnter == null) {
            return;
        }

        tEnter(collider);
    }
    private void OnTriggerExit(Collider collider) {

        if (tExit == null) {
            return;
        }

        tExit(collider);
    }
}
