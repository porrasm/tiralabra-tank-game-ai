using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBullet : MonoBehaviour {

    public TankPlayer Owner { get; set; }
    private int damage;
    private float speed;

    private Rigidbody rb;
    private Vector3 velocity;

    private float harmlessTime = 0.3f;
    private float aliveTime;
    private int bounces;

    private void Awake() {
        transform.position = transform.position += transform.forward * speed * Time.deltaTime;
    }
    private void Start() {
        damage = TankSettings.BulletDamage;
        speed = TankSettings.BulletSpeed;
        rb = GetComponent<Rigidbody>();

        velocity = transform.forward * speed;
        transform.eulerAngles = Vector3.zero;

        aliveTime = TankSettings.BulletAliveTime;
        bounces = TankSettings.BulletBounces;
    }

    private void Update() {
        UpdateTime();
        FixVelocity();
    }
    private void UpdateTime() {

        if (aliveTime > 0) {
            aliveTime -= Time.deltaTime;
        } else {
            Kill();
        }

        if (harmlessTime > 0) {
            harmlessTime -= Time.deltaTime;
        }  
    }
    private void FixVelocity() {
        rb.velocity = velocity;
        rb.angularVelocity = Vector3.zero;
    }

    public void OnCollisionEnter(Collision collision) {
        
        if (HitPlayer(collision.gameObject)) {
            return;
        }

        if (bounces == 0) {
            Kill();
        }

        Bounce(collision.contacts[0]);

        damage -= TankSettings.BulletDamageBounceReduction;
        bounces--;
    }
    private void Bounce(ContactPoint point) {
        Vector3 newDirection = Vector3.Reflect(velocity, point.normal);
        velocity = newDirection.normalized * speed; 
    }

    private void Kill() {
        Destroy(gameObject);
    }

    private bool HitPlayer(GameObject obj) {   

        TankPlayer p = obj.GetComponent<TankPlayer>();

        if (p == null) {
            return false;
        }

        if (harmlessTime > 0) {
            if (p.Equals(Owner)) {
                return true;
            }
        }

        p.DoDamage(damage, Owner);
        Kill();
        return true;
    }
}
