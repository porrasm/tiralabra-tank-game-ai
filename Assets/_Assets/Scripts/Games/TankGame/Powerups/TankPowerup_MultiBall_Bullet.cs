using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TankPowerup_MultiBall_Bullet : TankBullet {

    private int tiltAngle = 30;
    private bool ignoreFirst;

    void Start() {
    }

    public void Initialize(Transform other, int bounces, int damage, Vector3 velocity, bool ignoreFirst) {
        transform.position = other.position;
        transform.eulerAngles = other.eulerAngles;
        speed = TankSettings.BulletSpeed;
        rb = GetComponent<Rigidbody>();

        this.Velocity = velocity.normalized * speed;

        AliveTime = TankSettings.BulletAliveTime * 3;
        Bounces = bounces;
        Damage = damage;

        this.ignoreFirst = ignoreFirst;

        if (ignoreFirst) {
            harmlessTime = 0.2f;
        }
    }

    protected override void CollisionHappened(Collision collision, bool fix) {

        if (HitPlayer(collision.gameObject)) {
            return;
        }

        Bounce(collision.contacts[0]);
        
        if (fix || harmlessTime > 0 || ignoreFirst) {
            ignoreFirst = false;
            return;
        }

        if (Bounces <= 0) {
            Kill();
            return;
        }

        if (!ConstantDamage) {
            Damage /= 2;
        }

        Bounces--;

        GameObject copy = Instantiate(gameObject);
        InitializeCopy(copy.GetComponent<TankPowerup_MultiBall_Bullet>());
        TiltDirection(true);
    }
    static int cp;

    private void InitializeCopy(TankPowerup_MultiBall_Bullet bullet) {

        bullet.Initialize(transform, Bounces, Damage, Velocity, true);

        bullet.TiltDirection(false);
    }
    private void TiltDirection(bool primary) {
        int angle = tiltAngle;

        if (!primary) {
            angle *= 2;
        } else {
            angle = 0;
        }

        Vector3 vel = Quaternion.Euler(0, angle, 0) * Velocity;

        SetDirection(vel);
    }
}
