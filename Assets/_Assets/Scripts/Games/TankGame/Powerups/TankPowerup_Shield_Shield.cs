using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPowerup_Shield_Shield : MonoBehaviour {

    private Transform player;
    private TankPlayer tPlayer;
    private new SphereCollider collider;
    private HashSet<GameObject> bullets;

    private bool grown;
    private Vector3 targetScale;
    private Color targetColor;

    // Start is called before the first frame update
    public void Initialize(Transform player) {
        this.player = player;
        tPlayer = player.GetComponent<TankPlayer>();
        collider = GetComponent<SphereCollider>();
        Physics.IgnoreCollision(collider, player.GetChild(0).GetComponent<BoxCollider>(), true);
        bullets = new HashSet<GameObject>();

        InitGrow();
    }
    private void InitGrow() {
        targetScale = transform.localScale;
        targetColor = GetComponent<Renderer>().material.color;

        transform.localScale = targetScale * 0.01f;
        //GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update() {
        transform.position = player.position;

        if (!grown) {
            Grow();
        }
    }
    private void Grow() {
        transform.localScale += Time.deltaTime * targetScale;
        Vector3 scale = transform.localScale;
        if (scale.x >= targetScale.x && scale.y >= targetScale.y && scale.z >= targetScale.z) {
            transform.localScale = targetScale;
            grown = true;
        }
    }

    private void OnTriggerEnter(Collider other) {

        Vector3 pos = other.transform.position;

        TankBullet bullet = other.GetComponent<TankBullet>();

        if (bullet == null) {
            print("no bullet");
            return;
        }

        if (bullet.Owner.gameObject == tPlayer.gameObject) {
            if (!bullets.Contains(bullet.gameObject)) {
                bullets.Add(bullet.gameObject);
                print("Own bullet, returning");
                //return;
            }
        }

        print("Shield bounce: " + GetNormal(pos));

        bullet.SetDirection(GetNormal(pos));
    }

    private bool InsideCollider(Vector3 pos) {
        return collider.bounds.Contains(pos);
    }

    private Vector3 GetNormal(Vector3 pos) {
        print("P pos: " + player.transform.position);
        print("B pos: " + pos);
        return ResetY(pos) - ResetY(player.position);
    }
    private Vector3 ResetY(Vector3 v) {
        v.y = 0;
        return v;
    }
}
