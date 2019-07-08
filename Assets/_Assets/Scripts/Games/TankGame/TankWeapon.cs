using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankWeapon : MonoBehaviour {


    #region fields
    private Transform bulletSpawn;
    private TankPlayer player;

    [SerializeField]
    private GameObject bulletPrefab;

    private TankPowerup powerup;
    public void NullifyPowerup() {
        powerup = null;
    }

    private int clip;
    private bool reloading;
    private bool fireWait;
    private float fireDelay;

    private float reloadTime;
    #endregion


    private void Start() {
        bulletSpawn = transform.GetChild(0).GetChild(2).GetChild(0);

        player = GetComponent<TankPlayer>();

        fireDelay = 1.0f / TankSettings.FireRate;

        Reload();

        powerup = TankPowerup.GivePowerup(TankPowerup.Type.Shield, gameObject);
    }

    private int fireIndex;
    private int powerupIndex;

    public void Fire(int index) {
        if (index > fireIndex) {
            if (!BlockFire()) {
                if (!fireWait) {
                    fireIndex = index;
                    FirePrefab();
                }
            } else {
                fireIndex = index;
            }
        }
    }
    private bool BlockFire() {
        if (clip < 1) {
            return true;
        }
        if (powerup == null) {
            return false;
        }

        if (powerup.BehaviourType == TankPowerup.Behaviour.OverrideFire) {
            Powerup(powerupIndex + 1);
            return false;
        }

        return powerup.BehaviourType == TankPowerup.Behaviour.BlockFire;
    }

    private GameObject FirePrefab() {
        return FirePrefab(bulletPrefab);
    }
    public GameObject FirePrefab(GameObject bulletPrefab) {

        clip--;

        Reload();
        WaitFire();

        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = bulletSpawn.position;
        newBullet.transform.forward = bulletSpawn.forward;
        newBullet.GetComponent<TankBullet>().Owner = player;

        return newBullet;
    }

    private void WaitFire() {

        if (fireWait) {
            return;
        }

        IEnumerator WaitCoroutine() {
            float time = fireDelay;

            while (time > 0) {
                time -= Time.deltaTime;
                yield return null;
            }

            fireWait = false;
        }

        fireWait = true;
        StartCoroutine(WaitCoroutine());
    }

    public void Reload() {

        if (reloading && clip > 0) {
            reloadTime = TankSettings.ReloadTime;
            return;
        } else if (reloading && clip == 0) {
            return;
        }

        IEnumerator ReloadCoroutine() {

            reloadTime = TankSettings.ReloadTime;

            while (reloadTime > 0) {
                reloadTime -= Time.deltaTime;
                yield return null;
            }

            reloading = false;

            clip = TankSettings.ClipAmount;
        }

        reloading = true;

        StartCoroutine(ReloadCoroutine());
    }

    public void Powerup(int index) {
        if (index > powerupIndex) {
            powerupIndex = index;

            if (powerup != null) {
                powerup.Use();
            }
        }
    }

    public void Set() {

        TankNetworking net = GetComponent<TankNetworking>();

        fireIndex = net.Fire;
        powerupIndex = net.Powerup;
    }
}
