using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {

    private TankPlayer player;

    [SerializeField]
    private float speed, rotateSpeed;

    private TankControls controls;

    private Transform bulletSpawn;
    private Rigidbody rb;

    [SerializeField]
    private GameObject bulletPrefab;

    private int fireIndex = 0;

    private void Start() {
        if (!Server.Networker.IsServer) {
            Destroy(this);
            return;
        }

        player = GetComponent<TankPlayer>();

        bulletSpawn = transform.GetChild(0).GetChild(2).GetChild(0);
        rb = GetComponent<Rigidbody>();
        controls = GetComponent<TankControls>();

        speed = TankSettings.TankSpeed;
        rotateSpeed = TankSettings.TankRotateSpeed;
    }
    private void Update() {
        StopTank();
        MoveTank();

        if (controls.Fire > fireIndex) {
            fireIndex = controls.Fire;
            GameObject newBullet = Instantiate(bulletPrefab);
            newBullet.transform.position = bulletSpawn.position;
            newBullet.transform.forward = bulletSpawn.forward;
            newBullet.GetComponent<TankBullet>().Owner = player;
        }
    }

    private void StopTank() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void MoveTank() {

        Vector3 velocity = controls.Movement.y * transform.forward * speed * Time.deltaTime;
        Vector3 eulerRotation = Vector3.up * controls.Movement.x * rotateSpeed;
        Quaternion deltaRotation = Quaternion.Euler(eulerRotation * Time.deltaTime);

        // transform.eulerAngles += Vector3.forward * rotation;
        rb.MoveRotation(rb.rotation * deltaRotation);
        rb.MovePosition(rb.position + velocity);
    }

}
