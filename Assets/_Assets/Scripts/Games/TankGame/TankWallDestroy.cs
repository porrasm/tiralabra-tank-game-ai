using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankWallDestroy : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

        IEnumerator ExplosionCoroutine() {
            List<GameObject> walls = new List<GameObject>();

            foreach (Transform child in transform) {

                walls.Add(child.gameObject);
            }

            yield return null;

            foreach (GameObject child in walls) {
                Rigidbody rb = child.AddComponent<Rigidbody>();
                Lifetime.SetLifetime(child, 5);

                child.transform.parent = null;

                child.GetComponent<BoxCollider>().size = Vector3.one * 0.1f;

                rb.AddExplosionForce(50, transform.position - Vector3.up, 10);
            }
        }

        StartCoroutine(ExplosionCoroutine());
    }

    // Update is called once per frame
    void Update() {

    }
}
