using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankWeapon : MonoBehaviour {

    private Transform bulletSpawn;
    private TankPlayer player;

    [SerializeField]
    private GameObject bulletPrefab;

    private int clip;
    private bool reloading;
    private bool fireWait;
    private float fireDelay;

    private void Start() {
        bulletSpawn = transform.GetChild(0).GetChild(2).GetChild(0);

        player = GetComponent<TankPlayer>();

        fireDelay = 1.0f / TankSettings.FireRate;

        Reload();
    }

    private int fireIndex;
    private int powerupIndex;

    public void Fire(int index) {
        if (index > fireIndex) {
            if (!reloading) {            
                if (!fireWait) {
                    fireIndex = index;
                    Fire();
                }
            } else {
                fireIndex = index;
            }
        }
    }
    private void Fire() {

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

            // Powerup instantiation
        }
    }

    public void Set() {

        TankNetworking net = GetComponent<TankNetworking>();

        fireIndex = net.Fire;
        powerupIndex = net.Powerup;
    }
}
