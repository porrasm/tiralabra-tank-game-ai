using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maybe use unity events/actions for powerups
public class TankPowerup_Charge : TankPowerup {

    private float powerupTime = TankSettings.P_ChargeTime;
    private float speed;
    private bool used;

    private void Start() {
        powerupType = Type.Charge;
        behaviourType = Behaviour.BlockFire;

        speed = TankSettings.P_ChargeSpeedFactor * TankSettings.TankSpeed;
    }

    public override void Use() {

        if (used) {
            return;
        }

        used = true;

        base.Use();

        IEnumerator ChargeCoroutine() {

            Rigidbody rb = tankObject.GetComponent<Rigidbody>();

            while (powerupTime > 0) {

                Vector3 velocity = rb.transform.forward * speed * Time.deltaTime;

                rb.MovePosition(rb.position + velocity);

                powerupTime -= Time.deltaTime;
                yield return null;
            }
        }

        StartCoroutine(ChargeCoroutine());
    }
}
