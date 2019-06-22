using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPowerup_Regenerate : TankPowerup {

    private bool used = false;

    private GameObject particle;

    public override void Use() {
        base.Use();

        Remove();

        particle = Instantiate(TankGameHost.Game().HealthRegenParticlePrefab);
        particle.transform.SetParent(transform);
        particle.transform.localPosition = new Vector3(0, 1, 0);

        TankNetworking net = GetComponent<TankNetworking>();

        IEnumerator RegenCoroutine() {
            
            float time = TankSettings.P_RegenerateTime;
            float amountFactor = TankSettings.P_RegenerateAmount / time;

            float amount = 0;

            while (time > 0) {

                time -= Time.deltaTime;

                amount += Time.deltaTime * amountFactor;

                if (amount > 1) {
                    net.Health += (int)amount;
                    amount -= (int)amount;
                }

                yield return null;
            }

            used = true;
        }

        StartCoroutine(RegenCoroutine());
    }

    public override void Remove() {

        IEnumerator RemoveCoroutine() {

            GetComponent<TankWeapon>().NullifyPowerup();

            while (!used) {
                yield return null;
            }

            Destroy(this);

            ParticleSystem par = particle.GetComponent<ParticleSystem>();
            var emission = par.emission;
            emission.rateOverTime = 0;

            Lifetime.SetLifetime(particle, par.main.startLifetime.constant);
        }

        StartCoroutine(RemoveCoroutine());
    }
}
