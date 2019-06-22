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
    #endregion


    private void Start() {
        bulletSpawn = transform.GetChild(0).GetChild(2).GetChild(0);

        player = GetComponent<TankPlayer>();

        fireDelay = 1.0f / TankSettings.FireRate;

        Reload();

        powerup = TankPowerup.GivePowerup(TankPowerup.Type.Regenerate, gameObject);
    }

    private int fireIndex;
    private int powerupIndex;

    public void Fire(int index) {
        if (index > fireIndex) {
            if (!BlockFire()) {
                if (!fireWait) {
                    fireIndex = index;
                    Fire();
                }
            } else {
                fireIndex = index;
            }
        }
    }
    private bool BlockFire() {
        if (reloading) {
            return true;
        }
        if (powerup == null) {
            return false;
        }

        return powerup.BlockFire();
    }

    private void Fire() {

        print(reloading);
        print(clip);

        clip--;

        if (clip <= 0) {
            Reload();
        } else {
            WaitFire();
        }

        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = bulletSpawn.position;
        newBullet.transform.forward = bulletSpawn.forward;
        newBullet.GetComponent<TankBullet>().Owner = player;
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

        if (reloading) {
            return;
        }

        IEnumerator ReloadCoroutine() {

            float time = TankSettings.ReloadTime;

            while (time > 0) {
                time -= Time.deltaTime;
                yield return null;
            }

            reloading = false;
        }

        reloading = true;

        clip = TankSettings.ClipAmount;

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
