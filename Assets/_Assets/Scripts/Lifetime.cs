using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour {

    private float lifetime;

    void Update() {
        lifetime -= Time.deltaTime;

        if (lifetime <= 0) {
            Destroy(gameObject);
        }
    }

    public static void SetLifetime(GameObject g, float t) {
        Lifetime l = g.AddComponent<Lifetime>();
        l.lifetime = t;
    }
}
