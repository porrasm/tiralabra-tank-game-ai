using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPowerup_Shield : TankPowerup {

    #region fields
    private new Behaviour behaviourType = Behaviour.OverrideFire;

    private GameObject shield;
    #endregion

    public override void Use() {
        base.Use();

        shield = Instantiate(TankGameHost.Game().ShieldPrefab);

        shield.GetComponent<TankPowerup_Shield_Shield>().Initialize(transform);
        Lifetime.SetLifetime(shield, TankSettings.P_ShieldTime);

        GetComponent<TankPlayer>().Invulnerable = false;

        Invoke("Remove", TankSettings.P_ShieldTime);
    }

    public override void Remove() {

        StopAllCoroutines();

        if (shield != null) {
            Destroy(shield);
        }

        GetComponent<TankPlayer>().Invulnerable = false;

        base.Remove();
    }
}
