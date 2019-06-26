using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPowerup_Missile : TankPowerup {

    private Transform crosshair, target;
    private TankNetworking targetPlayer;
    private float rotateSpeed = 100;
    private bool used;
    private bool locked;

    private float initTime = 1;
    private float time, targetTime;

    void Update() {
        MoveCrosshair();
    }

    public override void Use() {
        base.Use();

        IEnumerator UseCoroutine() {

            used = true;

            InitializeCrosshair();

            yield return new WaitForSeconds(initTime);

            SeekPlayer();

            yield return new WaitForSeconds(time);

            print("Locked player");

            yield return new WaitUntil(CrosshairOnTarget);

            locked = true;

            print("locked");

            ShootLaser();
        }

        StartCoroutine(UseCoroutine());
    }

    private void InitializeCrosshair() {

        crosshair = Instantiate(TankGameHost.Game().PowerupCrosshairObject).transform;
        float pos = 0.5f * TankSettings.LevelWidth;
        crosshair.position = new Vector3(pos, 1, pos);

        float time = 0;
        float factor = 0.01f;

        Light light = crosshair.GetChild(0).GetComponent<Light>();

        IEnumerator InitCoroutine() {       

            while (time < initTime) {

                time += Time.deltaTime;

                factor = time / initTime;

                if (factor > 1) {
                    factor = 1;
                }

                crosshair.localScale = Vector3.one * factor;
                light.intensity = factor * 0.5f;

                yield return null;
            }

            crosshair.localScale = Vector3.one;
            light.intensity = 0.5f;
        }

        StartCoroutine(InitCoroutine());
    }

    private void SeekPlayer() {

        target = RandomPlayer();

        time = TankSettings.P_MissileTime * 0.75f + TankSettings.P_MissileTime * 0.5f;
        targetTime = RandomTargetTime();

        IEnumerator SeekCoroutine() {

            while (time > 0) {

                time -= Time.deltaTime;
                targetTime -= Time.deltaTime;

                if (targetTime <= 0) {
                    targetTime = RandomTargetTime();
                    target = RandomPlayer();
                }

                yield return null;
            }
        }

        StartCoroutine(SeekCoroutine());
    }

    private void ShootLaser() {

        target.GetComponent<TankPlayer>().Invulnerable = false;

        IEnumerator CrosshairLock() {

            float scaleBig = 0.25f;
            float scaleSmall = -0.5f;

            float time = 0;
            float scaleFactor = 1 / time;

            while (time < 0.25f) {

                time += Time.deltaTime;

                float factor = time / 0.25f;

                crosshair.localScale = Vector3.one * (1 + factor * scaleBig);
                yield return null;
            }

            rotateSpeed *= 4;

            time = 0;
            while (time < 0.33f) {

                time += Time.deltaTime;

                float factor = time / 0.33f;

                crosshair.localScale = Vector3.one * (1 + scaleBig + factor * scaleSmall);
                yield return null;
            }
        }

        StartCoroutine(CrosshairLock());
    }

    private Transform RandomPlayer() {

        print("Switch missile target");

        GameObject[] tanks = GameObject.FindGameObjectsWithTag("Player");

        List<GameObject> alive = new List<GameObject>();

        foreach (GameObject g in tanks) {
            if (g.GetComponent<TankNetworking>().State == TankPlayer.PlayerState.Enabled) {
                alive.Add(g);
            }
        }

        GameObject p = alive[Random.Range(0, alive.Count - 1)];
        targetPlayer = p.GetComponent<TankNetworking>();
        return p.transform;
    }
    private float RandomTargetTime() {
        return 0.25f + Random.value * (TankSettings.P_MissileTargetChangeMax - 0.25f);
    }

    private void MoveCrosshair() {

        if (!used) {
            return;
        }

        crosshair.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        crosshair.position += TranslateDirection();
    }

    private Vector3 TranslateDirection() {

        if (target == null) {
            return Vector3.zero;
        }

        float distance = XZDistance();

        Vector3 direction = (target.position - crosshair.position);
        direction.y = 0;

        direction = direction.normalized * TankSettings.P_MissileSpeed * Time.deltaTime;

        if (direction.magnitude > distance) {
            direction = direction.normalized * distance;
        }

        return direction;
    }
    private bool CrosshairOnTarget() {

        if (targetPlayer.State != TankPlayer.PlayerState.Enabled) {
            print("Target not alive");
            target = RandomPlayer();
        }

        return XZDistance() < 0.2f;
    }
    private float XZDistance() {

        Vector3 ch = crosshair.position;

        ch.y = target.position.y;

        return Vector3.Distance(target.position, ch);
    }

    public override void Remove() {

        StopAllCoroutines();

        Destroy(crosshair.gameObject);

        base.Remove();
    }
}
