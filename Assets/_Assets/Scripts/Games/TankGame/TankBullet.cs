using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBullet : MonoBehaviour {

    public TankPlayer Owner { get; set; }
    private int damage;
    private float speed;

    private Rigidbody rb;
    private Vector3 velocity;

    private void Start() {
        damage = TankSettings.BulletDamage;
        speed = TankSettings.BulletSpeed;
        rb = GetComponent<Rigidbody>();

        velocity = transform.forward * speed;
        transform.eulerAngles = Vector3.zero;
    }

    private void Update() {
        FixVelocity();
    }
    private void FixVelocity() {
        rb.velocity = velocity;
        rb.angularVelocity = Vector3.zero;
    }

    public void OnCollisionEnter(Collision collision) {
        
        if (HitPlayer(collision.gameObject)) {
            return;
        }

        print("Count: " + collision.contactCount);

        Bounce(collision.contacts[0]);

        damage -= TankSettings.BulletDamageBounceReduction;
    }
    private void Bounce(ContactPoint point) {
        Vector3 newDirection = Vector3.Reflect(velocity, point.normal);
        velocity = newDirection.normalized * speed; 
    }


    private bool HitPlayer(GameObject obj) {

        TankPlayer p = obj.GetComponent<TankPlayer>();

        if (p == null) {
            return false;
        }

        print("Hit player, doing damage");

        p.DoDamage(damage, Owner);
        Destroy(this.gameObject);
        return true;
    }
}
