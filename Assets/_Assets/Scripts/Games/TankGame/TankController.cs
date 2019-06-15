using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {

    #region fields
    private TankNetworking net;
    private TankPlayer player;

    [SerializeField]
    private float speed, rotateSpeed;

    private TankControls controls;

    private Transform bulletSpawn;
    private Rigidbody rb;

    [SerializeField]
    private GameObject bulletPrefab;

    private int fireIndex = 0;
    #endregion


    private void Start() {

        player = GetComponent<TankPlayer>();

        bulletSpawn = transform.GetChild(0).GetChild(2).GetChild(0);
        rb = GetComponent<Rigidbody>();
        controls = GetComponent<TankControls>();

        speed = TankSettings.TankSpeed;
        rotateSpeed = TankSettings.TankRotateSpeed;

        net = GetComponent<TankNetworking>();

        if (!Server.Networker.IsServer) {
            Destroy(this);
            return;
        }
    }
    public void SetMaterial() {

        Material material = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material;

        string textureName = "text_tank_body" + (net.Owner.ID + 1);
        Texture tex = Resources.Load<Texture>("TankGame/Textures/" + textureName);

        if (tex == null) {
            print("Texture was null");
        }

        print("Setting " + net.Owner.Name + " color to " + net.Owner.Color + " " + textureName);
        material.SetTexture(textureName, tex);
        material.color = Colors.GetBasicColor(net.Owner.Color);
    }

    private void Update() {

        if (net.State != TankPlayer.PlayerState.Enabled) {
            return;
        }

        StopTank();
        MoveTank();

        if (net.Fire > fireIndex) {
            fireIndex = net.Fire;
            GameObject newBullet = Instantiate(bulletPrefab);
            newBullet.transform.position = bulletSpawn.position;
            newBullet.transform.forward = bulletSpawn.forward;
            newBullet.GetComponent<TankBullet>().Owner = player;
        }
    }

    public void StopTank() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void MoveTank() {

        Vector3 velocity = net.Movement.y * transform.forward * speed * Time.deltaTime;
        Vector3 eulerRotation = Vector3.up * net.Movement.x * rotateSpeed;
        Quaternion deltaRotation = Quaternion.Euler(eulerRotation * Time.deltaTime);

        // transform.eulerAngles += Vector3.forward * rotation;
        rb.MoveRotation(rb.rotation * deltaRotation);
        rb.MovePosition(rb.position + velocity);
    }
}
