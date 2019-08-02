using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {

    #region fields
    private TankNetworking net;
    private TankPlayer player;

    [SerializeField]
    private float speed, rotateSpeed;
    public void SetSpeeds(float speed, float rotateSpeed) {
        this.speed = speed;
        this.rotateSpeed = rotateSpeed;
    }

    private TankControls controls;
    private TankWeapon weapon;

    public bool BlockMove { get; set; }

    private Rigidbody rb;

    [SerializeField]
    private GameObject bulletPrefab;

    private int fireIndex = 0;
    #endregion


    private void Start() {

        player = GetComponent<TankPlayer>();

        
        rb = GetComponent<Rigidbody>();
        controls = GetComponent<TankControls>();
        weapon = GetComponent<TankWeapon>();

        speed = TankSettings.TankSpeed;
        rotateSpeed = TankSettings.TankRotateSpeed;

        net = GetComponent<TankNetworking>();

        if (Server.Networker != null && !Server.Networker.IsServer) {
            Destroy(this);
            return;
        }
    }
    public void SetMaterial() {

        Material material = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material;

        string textureName = "text_tank_body" + (net.Owner.ID + 1);
        Texture tex = Resources.Load<Texture>("TankGame/Textures/" + textureName);

        print("Setting " + net.Owner.Name + " color to " + net.Owner.Color + " " + textureName);
        material.SetTexture(textureName, tex);
        material.color = Colors.GetBasicColor(net.Owner.Color);
    }

    private void Update() {

        if (net.State != TankPlayer.PlayerState.Enabled) {
            return;
        }

        FireAndPowerup();
        StopTank();
        MoveTank();
    }

    public void StopTank() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void MoveTank() {

        if (BlockMove) {
            return;
        }

        Vector3 velocity = net.Movement.y * transform.forward * speed * Time.deltaTime;
        Vector3 eulerRotation = Vector3.up * net.Movement.x * rotateSpeed;
        Quaternion deltaRotation = Quaternion.Euler(eulerRotation * Time.deltaTime);

        // transform.eulerAngles += Vector3.forward * rotation;
        rb.MoveRotation(rb.rotation * deltaRotation);
        rb.MovePosition(rb.position + velocity);
    }
    private void FireAndPowerup() {
        weapon.Fire(net.Fire);
        weapon.Powerup(net.Powerup);
    }
}
