using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBullet : MonoBehaviour {

    #region fields
    public TankPlayer Owner { get; set; }
    public int Bounces { get => bounces; set => bounces = value; }
    public float AliveTime { get => aliveTime; set => aliveTime = value; }
    public bool ConstantDamage { get; set; }
    public int Damage { get => damage; set => damage = value; }
    public Vector3 Velocity { get => velocity; set => velocity = value; }

    protected int damage;
    protected float speed;

    protected Rigidbody rb;
    private Vector3 velocity;

    protected float harmlessTime = 0.3f;
    protected float aliveTime;
    protected int bounces;

    protected int collisionFrames;
    #endregion


    private void Awake() {
        transform.position = transform.position += transform.forward * speed * Time.deltaTime;
    }
    private void Start() {

        Damage = TankSettings.BulletDamage;
        speed = TankSettings.BulletSpeed;
        rb = GetComponent<Rigidbody>();

        Velocity = transform.forward * speed;

        AliveTime = TankSettings.BulletAliveTime;
        Bounces = TankSettings.BulletBounces;

        print("Calling event");
        TankEvents.Instance.CallEvent(TankEvents.EventType.BulletEvent);
    }

    public void SetDirection(Vector3 direction) {
        direction.y = 0;
        Velocity = direction.normalized * speed;
    }

    private void Update() {
        UpdateTime();
        FixVelocity();
    }
    private void UpdateTime() {

        if (AliveTime > 0) {
            AliveTime -= Time.deltaTime;
        } else {
            Kill();
        }

        if (harmlessTime > 0) {
            harmlessTime -= Time.deltaTime;
        }
    }
    private void FixVelocity() {
        rb.velocity = Velocity;
        rb.angularVelocity = Vector3.zero;
    }

    #region Collision
    protected void OnCollisionEnter(Collision collision) {
        collisionFrames = 0;
        CollisionHappened(collision, false);
        TankEvents.Instance.CallEvent(TankEvents.EventType.BulletEvent);
    }
    protected void OnCollisionStay(Collision collision) {
        collisionFrames++;
        if (collisionFrames > 2) {
            FixCollision(collision);
        }
    }
    protected void OnCollisionExit(Collision collision) {
        collisionFrames = 0;
    }
    protected virtual void FixCollision(Collision collision) {
        if (collisionFrames > 6) {
            Vector3 rnd = new Vector3(Random.value, 0, Random.value);
            collisionFrames = 0;
            SetDirection(rnd);
        } else {
            CollisionHappened(collision, true);
        }
    }
    protected virtual void CollisionHappened(Collision collision, bool fix) {
        if (HitPlayer(collision.gameObject)) {
            return;
        }

        if (Bounces == 0) {
            Kill();
        }

        Bounce(collision.contacts[0].normal);

        if (!ConstantDamage) {
            Damage -= TankSettings.BulletDamageBounceReduction;
        }

        if (!fix) {
            Bounces--;
        }
    }
    public void Bounce(Vector3 normal) {
        Vector3 newDirection = Vector3.Reflect(Velocity, normal);
        Velocity = newDirection.normalized * speed;
    }

    protected void Kill() {
        Destroy(gameObject);
    }

    protected bool HitPlayer(GameObject obj) {

        TankPlayer p = obj.GetComponent<TankPlayer>();

        if (p == null) {
            return false;
        }

        if (harmlessTime > 0) {
            if (p.Equals(Owner)) {
                return true;
            }
        }

        p.DoDamage(Damage, Owner);
        Kill();
        return true;
    }
    #endregion
}
