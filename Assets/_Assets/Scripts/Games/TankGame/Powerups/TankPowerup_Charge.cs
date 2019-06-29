using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maybe use unity events/actions for powerups
public class TankPowerup_Charge : TankPowerup {

    private float powerupTime = TankSettings.P_ChargeTime;
    private float speed;
    private bool used;

    ColliderCallback colliderCallback;

    private void Start() {
        powerupType = Type.Charge;
        behaviourType = Behaviour.BlockFire;

        speed = TankSettings.P_ChargeSpeedFactor * TankSettings.TankSpeed;
        colliderCallback = gameObject.AddComponent<ColliderCallback>();
        colliderCallback.AddCollisionCallback(CollisionCallback);
    }

    public override void Remove() {
        base.Remove();

        GetComponent<TankPlayer>().Invulnerable = false;

        Destroy(this);
        Destroy(colliderCallback);
    }

    private void CollisionCallback(GameObject obj, Collision collision) {

        if (!used) {
            return;
        }

        TankPlayer player = collision.gameObject.GetComponent<TankPlayer>();
        if (player == null) {
            return;
        }

        player.DoDamage(TankSettings.P_ChargeDamage, GetComponent<TankPlayer>());

        print("Collision callback");
    }

    public override void Use() {

        if (used) {
            return;
        }

        used = true;

        base.Use();

        GetComponent<TankPlayer>().Invulnerable = true;

        IEnumerator ChargeCoroutine() {

            Rigidbody rb = tankObject.GetComponent<Rigidbody>();

            while (powerupTime > 0) {

                Vector3 velocity = rb.transform.forward * speed * Time.deltaTime;

                rb.MovePosition(rb.position + velocity);

                powerupTime -= Time.deltaTime;
                yield return null;
            }

            Remove();
        }

        StartCoroutine(ChargeCoroutine());
    }

    public override bool BlockFire() {
        return used;
    }
}
