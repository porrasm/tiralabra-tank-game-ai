using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlane : MonoBehaviour {

    [SerializeField]
    private float distance, speed;

    private int direction = 1;

    private float height;

    private Cloth cloth;

    private void Start() {
        height = transform.position.y;
        cloth = GetComponent<Cloth>();
    }

    private void Update() {

        

        transform.Translate(Vector3.up * direction * speed * Time.deltaTime);

        if (direction > 0) {
            if (transform.position.y - height >= distance) {
                direction *= -1;
            }
        } else {
            if (height - transform.position.y >= distance) {
                direction *= -1;
            }
        }
    }
}
