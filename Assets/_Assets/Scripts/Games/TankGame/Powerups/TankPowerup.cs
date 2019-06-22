using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TankPowerup : MonoBehaviour {

    public enum Type { Charge, Regenerate }
    public enum Behaviour { Passive, Fire, BlockFire }

    protected GameObject tankObject;

    protected Type powerupType;
    protected Behaviour behaviourType;

    public Behaviour BehaviourType { get => behaviourType; set => behaviourType = value; }
    public Type PowerupType { get => powerupType; set => powerupType = value; }

    public virtual void Use() {
        print("Using powerup: " + powerupType);
    }
    public virtual void Remove() {
        Destroy(this);
    }
    public virtual bool BlockFire() {
        return false;
    }
    public virtual bool CustomFire() {
        return false;
    }

    public void SetTankObject(GameObject tank) {
        tankObject = tank;
    }

    #region Set Powerups
    public static TankPowerup GivePowerup(Type type, GameObject tank) {

        RemovePowerup(tank);

        switch (type) {
            case Type.Charge:
                return GiveCharge(tank);
            case Type.Regenerate:
                return GiveRegenerate(tank);
        }

        return null;
    }
    public static void RemovePowerup(GameObject tank) {

        TankPowerup p = tank.GetComponent<TankPowerup>();

        if (p != null) {
            p.Remove();
        }
    }

    private static TankPowerup GiveCharge(GameObject tank) {

        TankPowerup_Charge powerup = tank.AddComponent<TankPowerup_Charge>();
        powerup.tankObject = tank;

        return powerup;
    }
    private static TankPowerup GiveRegenerate(GameObject tank) {

        TankPowerup_Regenerate powerup = tank.AddComponent<TankPowerup_Regenerate>();
        powerup.tankObject = tank;

        return powerup;
    }
    #endregion
}
