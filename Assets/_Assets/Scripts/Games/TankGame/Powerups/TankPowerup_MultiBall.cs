using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TankPowerup_MultiBall : TankPowerup {

    private int bounces = 8;

    public override void Use() {
        base.Use();

        TankWeapon weapon = GetComponent<TankWeapon>();

        TankPowerup_MultiBall_Bullet bullet = weapon.FirePrefab(TankGameHost.Game().MultiballPrefab).GetComponent<TankPowerup_MultiBall_Bullet>();
        int damage = TankSettings.P_MultiBall_Damage * (int)Mathf.Pow(2, bounces - 1);

        bullet.Initialize(bullet.transform, bounces, damage, bullet.transform.forward, false);
        print("multiball fired: " + bullet);
    }

    public override void Remove() {

        IEnumerator RemoveCoroutine() {

            yield return null;

            base.Remove();
        }

        StartCoroutine(RemoveCoroutine());
    }

    private void AddMultiball(TankBullet bullet, int bouncesLeft) {

        bullet.Bounces = bouncesLeft;
        var cb = bullet.gameObject.AddComponent<ColliderCallback>();
        cb.AddCollisionCallback(OnBulletCollision);
    }

    private void OnBulletCollision(GameObject obj, Collision collision) {
        
        

    }
}