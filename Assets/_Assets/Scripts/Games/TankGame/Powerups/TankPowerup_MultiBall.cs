using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPowerup_MultiBall : TankPowerup {

    private int bounces = 8;

    public override void Use() {
        base.Use();

        TankWeapon weapon = GetComponent<TankWeapon>();

        TankPowerup_MultiBall_Bullet bullet = weapon.FirePrefab(TankGameHost.Game().MultiballPrefab).GetComponent<TankPowerup_MultiBall_Bullet>();
        int damage = TankSettings.P_MultiBall_Damage * (int)Mathf.Pow(2, bounces);

        bullet.Initialize(GetComponent<TankPlayer>(), bullet.transform, bounces, damage, bullet.transform.forward, false);
        print("multiball fired: " + bullet);
    }

    private void AddMultiball(TankBullet bullet, int bouncesLeft) {

        bullet.Bounces = bouncesLeft;
        var cb = bullet.gameObject.AddComponent<ColliderCallback>();
        cb.AddCollisionCallback(OnBulletCollision);
    }

    private void OnBulletCollision(GameObject obj, Collision collision) {
    }
}